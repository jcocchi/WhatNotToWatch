using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WhatNotToWatch.Models;
using WhatNotToWatch.Services;

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
