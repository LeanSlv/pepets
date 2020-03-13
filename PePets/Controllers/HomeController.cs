using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PePets.Models;

namespace PePets.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AdvertRepository _advertRepository;

        public HomeController(ILogger<HomeController> logger, AdvertRepository advertRepository)
        {
            _logger = logger;
            _advertRepository = advertRepository;
        }

        public IActionResult Index()
        {
            var adverts = _advertRepository.GetAdverts();
            return View(adverts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
