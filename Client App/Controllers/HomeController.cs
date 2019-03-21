using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Client_App.Models;
using Client_App.Services;
using Microsoft.AspNetCore.Authorization;

namespace Client_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAppMemoryCache _memoryCache;

        public HomeController(IAppMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "DeveloperToolPolicy")]
        public IActionResult Login()
        {
            ViewData["Message"] = "Your application authorization page.";

            return View();
        }

        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
