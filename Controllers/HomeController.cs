using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WhatNotToWatch.Models;
using WhatNotToWatch.Services;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        public IActionResult Create(TVShow tvShow)
        {
            if (ModelState.IsValid)
            {
                _tvShowData.Add(tvShow);

                return RedirectToAction("Index");
            }
            return View();
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
