using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WhatNotToWatch.Models;
using WhatNotToWatch.Services;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WhatNotToWatch.App_Code;

namespace WhatNotToWatch.Controllers
{
    public class HomeController : Controller
    {
        private ITvShowData _tvShowData;

        public HomeController(ITvShowData tvData)
        {
            _tvShowData = tvData;
        }

        public IActionResult Index(string sortOrder)
        {
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["GenreSortParam"] = sortOrder == "Genre" ? "genre_desc" : "Genre";
            ViewData["RatingSortParam"] = sortOrder == "Rating" ? "rating_desc" : "Rating";
            ViewData["VoteSortParam"] = sortOrder == "Vote" ? "vote_desc" : "Vote";

            var shows = from s in _tvShowData.GetAll() select s;

            switch (sortOrder)
            {
                case "name_desc":
                    shows = shows.OrderByDescending(s => s.Name);
                    break;
                case "Genre":
                    shows = shows.OrderBy(s => s.Genre);
                    break;
                case "genre_desc":
                    shows = shows.OrderByDescending(s => s.Genre);
                    break;
                case "Rating":
                    shows = shows.OrderBy(s => s.Rating);
                    break;
                case "rating_desc":
                    shows = shows.OrderByDescending(s => s.Rating);
                    break;
                case "Vote":
                    shows = shows.OrderBy(s => s.Vote);
                    break;
                case "vote_desc":
                    shows = shows.OrderByDescending(s => s.Vote);
                    break;
                default:
                    shows = shows.OrderBy(s => s.Name);
                    break;
            }

            return View(shows.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TVShow tvShow)
        {
            if (ModelState.IsValid)
            {
                // Calculate "Rating" by calling to MS Cognitve Services Text Analytics API 
                String results= await MakeRequest(tvShow.Review);
                System.Diagnostics.Debug.WriteLine("RESULTS: " + results);

                // Parse response and select rating score
                int rating = ParseResponse(results);
                System.Diagnostics.Debug.WriteLine("RATING: " + rating);

                // If there was a problem with the rating assignment take the user back to the Create page to try again
                if (rating == 0)
                {
                    return View();
                }

                // Otherwise set the rating and add the show to the DB
                tvShow.Rating = rating;
                _tvShowData.Add(tvShow);

                return RedirectToAction("Index");
            }
            return View();
        }

        private int ParseResponse(string results)
        {
            // JSON object to store API response
            ReviewResponse review = new ReviewResponse();
            double ratingRaw = -1;
            int rating = -1;

            // Populate a JSON object with the results of the API call
            JsonConvert.PopulateObject(results, review);

            ratingRaw = review.getScore();

            // Check if the raw rating came back correctly from the API and assign a rating from 1-10
            if (ratingRaw == -1)
            {
                rating = 0; // A rating of 0 indicates that the API returned an error
            }
            else if (ratingRaw > .95)
            {
                rating = 10;
            }
            else if (ratingRaw > .9)
            {
                rating = 9;
            }
            else if (ratingRaw > .8)
            {
                rating = 8;
            }
            else if (ratingRaw > .7)
            {
                rating = 7;
            }
            else if (ratingRaw > .6)
            {
                rating = 6;
            }
            else if (ratingRaw > .5)
            { 
                rating = 5;
            }
            else if (ratingRaw > .4)
            {
                rating = 4;
            }
            else if (ratingRaw > .3)
            {
                rating = 3;
            }
            else if (ratingRaw > .2)
            {
                rating = 2;
            }
            else
            {
                rating = 1;
            }

            return rating;
        }

        private async Task<String> MakeRequest(string review)
        {
            //var client = new WebClient(); System.Net
            var client = new HttpClient();
            var queryString = Request.Query;

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "4b2610a638a04140baf45aa53f8f5ba3"); 

var uri = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment?" + queryString;

            // Format body
            var jsonPrepend = "{\n \t \"documents\": [ \n\t\t {  \n\t\t\t \"language\": \"en\", \n\t\t\t \"id\" : \"1\", \n\t\t\t \"text\": \"";
            var jsonAppend = "\" \n\t\t } \n\t ] \n }";

            System.Diagnostics.Debug.WriteLine("JSON REQUEST: " + jsonPrepend + review + jsonAppend);

            HttpResponseMessage response; 

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(jsonPrepend + review + jsonAppend);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
            }

            // Convert response to string
            HttpContent results = response.Content;
            var res1 = await results.ReadAsStringAsync();

            return res1;
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _tvShowData.Get(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            } 
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(TVShow input)
        {
            var tvShow = _tvShowData.Get(input.ID);
            ModelValidationState isValid = ModelState.GetValidationState("Vote");

            if (tvShow != null && isValid.Equals(ModelValidationState.Valid))
            {
                tvShow.Vote = input.Vote;

                if(tvShow.Vote <= -5)
                {
                    Delete(tvShow);
                }

                _tvShowData.Update();
                
                return RedirectToAction("Index");
            }

            return View(tvShow);
        }

        
        private void Delete(TVShow showToDelete)
        {
            _tvShowData.Delete(showToDelete);
        }

        public IActionResult Index_Default()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
