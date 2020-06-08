using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PePets.Services;
using PePets.Models;

namespace PePets.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchService _searchService;
        private readonly AdvertRepository _advertRepository;

        public SearchController(SearchService searchService, AdvertRepository advertRepository)
        {
            _searchService = searchService;
            _advertRepository = advertRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string input)
        {
            List<Guid> ids = await _searchService.Search(input);

            List<Advert> adverts = new List<Advert>();
            foreach (Guid id in ids)
                adverts.Add(_advertRepository.GetAdvertById(id));

            return ViewComponent("AdvertsList", adverts);
        }
    }
}