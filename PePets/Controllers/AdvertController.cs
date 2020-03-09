using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Controllers
{
    public class AdvertController : Controller
    {
        private readonly AdvertRepository _advertRepository;

        public AdvertController(AdvertRepository advertRepository)
        {
            _advertRepository = advertRepository;
        }
        public IActionResult AdvertEdit(Guid id)
        {
            Advert advert = id == default ? new Advert() : _advertRepository.GetAdvertById(id);

            return View(advert);
        }

        [HttpPost]
        public IActionResult AdvertEdit(Advert advert)
        {
            if (ModelState.IsValid)
            {
                _advertRepository.SaveAdvert(advert);
                return RedirectToAction("Index", "Home");
            }

            return View(advert);
        }
    }
}
