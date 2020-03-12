using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Controllers
{
    public class AdvertController : Controller
    {
        private readonly AdvertRepository _advertRepository;
        IWebHostEnvironment _appEnvironment;

        public AdvertController(AdvertRepository advertRepository, IWebHostEnvironment appEnvironment)
        {
            _advertRepository = advertRepository;
            _appEnvironment = appEnvironment;
        }
        public IActionResult AdvertEdit(Guid id)
        {
            Advert advert = id == default ? new Advert() : _advertRepository.GetAdvertById(id);

            return View(advert);
        }

        [HttpPost]
        public IActionResult AdvertEdit(Advert advert, IFormFileCollection images)
        {
            if (ModelState.IsValid)
            {
                _advertRepository.SaveAdvert(advert);

                // Сохранение изображений в отдельную папку и добавление их путей в БД
                Directory.CreateDirectory($"{_appEnvironment.WebRootPath}/img/{advert.Id}");
                List<string> imagesPaths = new List<string>();
                int i = 0;
                foreach (var image in images)
                {
                    using (var fileStream = new FileStream($"{_appEnvironment.WebRootPath}/img/{advert.Id}/image{i}.png", FileMode.Create, FileAccess.Write))
                    {
                        image.CopyTo(fileStream);
                        imagesPaths.Add($"/img/{advert.Id}/image{i}.png");
                    }
                    i++;
                }
                advert.Images = imagesPaths.ToArray();
                _advertRepository.SaveAdvert(advert);

                return RedirectToAction("Index", "Home");
            }

            return View(advert);
        }
    }
}
