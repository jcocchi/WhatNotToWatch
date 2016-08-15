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

        public IActionResult Index()
        {
            return View(_tvShowData.GetAll());
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
