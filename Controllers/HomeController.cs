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
        public IActionResult Index()
        {
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

        public IActionResult Error()
        {
            return View();
        }
    }
}
