using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WhatNotToWatch.Services;
using WhatNotToWatch.Models;
using WhatNotToWatch.App_Code;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WhatNotToWatch.ViewModels;

namespace WhatNotToWatch.Controllers
{
    public class ReviewsController : Controller
    {
        private ITvShowData _tvShowData;

        public ReviewsController(ITvShowData tvData)
        {
            _tvShowData = tvData;
        }

        public IActionResult Index(string sortOrder, string searchString)
        {
            var model = new BrowseReviewsViewModel();

            model.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            model.GenreSortParam = sortOrder == "Genre" ? "genre_desc" : "Genre";
            model.RatingSortParam = sortOrder == "rating_desc" ? "Rating" : "rating_desc";
            model.VoteSortParam = sortOrder == "vote_desc" ? "Vote" : "vote_desc";

            var shows = from s in _tvShowData.GetAll() select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                shows = shows.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
            }

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

            model.Shows = shows.ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateShowViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tvShow = new TVShow();

                // Store attributes from the model in the new show
                tvShow.Name = model.Name;
                tvShow.Genre = model.Genre;
                tvShow.Review = model.Review;
                
                // Calculate "Rating" by calling to MS Cognitve Services Text Analytics API 
                String results = await MakeRequest(tvShow.Review);
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

                return RedirectToAction("Details", tvShow);
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
            // Ensure there are no reserved characters in the string submitted by the user
            // If there are, replace them with the proper character before converting to JSON
            if (review.Contains("\\"))
            {
                review = review.Replace("\\", "\\\\");
            }
            if (review.Contains("\"")){
                review= review.Replace("\"", "\\\"");
            }

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
        public IActionResult Details(DetailsViewModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
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
        public IActionResult Edit(EditViewModel model)
        {
            var tvShow = _tvShowData.Get(model.ID);

            if(ModelState.IsValid)
            {
                tvShow.Vote = model.Vote;

                if (tvShow.Vote <= -5)
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
    }
}
