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
        private readonly int maxImagesCount;

        public AdvertController(AdvertRepository advertRepository, IWebHostEnvironment appEnvironment)
        {
            _advertRepository = advertRepository;
            _appEnvironment = appEnvironment;
            maxImagesCount = 10;
        }
        public IActionResult AdvertEdit(Guid id)
        {
            Advert advert = id == default ? new Advert() : _advertRepository.GetAdvertById(id);

            return View(advert);
        }

        [HttpPost]
        [RequestSizeLimit(31457280)] // 30 Мб
        public IActionResult AdvertEdit(Advert advert, IFormFileCollection images)
        {
            if (images.Count > maxImagesCount)
                ModelState.AddModelError(nameof(advert.Images), "Нельзя загружать больше 10 изображений");
            if (!ModelState.IsValid)
                return View(advert);

            _advertRepository.SaveAdvert(advert);

                // Сохранение изображений в отдельную папку и добавление их путей в БД
                Directory.CreateDirectory($"{_appEnvironment.WebRootPath}/usersFiles/advertsImages/{advert.Id}");
                List<string> imagesPaths = new List<string>();
                int i = 0;
                foreach (var image in images)
                {
                    using (var fileStream = new FileStream($"{_appEnvironment.WebRootPath}/usersFiles/advertsImages/{advert.Id}/image{i}.png", FileMode.Create, FileAccess.Write))
                    {
                        image.CopyTo(fileStream);
                        imagesPaths.Add($"/usersFiles/advertsImages/{advert.Id}/image{i}.png");
                    }
                    i++;
                }
                advert.Images = imagesPaths.ToArray();
                _advertRepository.SaveAdvert(advert);

                return RedirectToAction("Index", "Home");
            }

            
        
    }
}
