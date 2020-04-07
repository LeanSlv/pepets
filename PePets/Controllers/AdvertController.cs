using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PePets.Controllers
{
    public class AdvertController : Controller
    {
        private readonly AdvertRepository _advertRepository;
        IWebHostEnvironment _appEnvironment;
        private readonly UserManager<User> _userManager;
        private readonly int maxImagesCount;

        public AdvertController(AdvertRepository advertRepository, IWebHostEnvironment appEnvironment, UserManager<User> userManager)
        {
            _advertRepository = advertRepository;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
            maxImagesCount = 10;
        }

        [Authorize]
        public IActionResult AdvertEdit(Guid id)
        {
            Advert advert = id == default ? new Advert() : _advertRepository.GetAdvertById(id);
            return View(advert);
        }

        [HttpPost]
        [RequestSizeLimit(31457280)] // 30 Мб
        public async Task<IActionResult> AdvertEdit(Advert advert, IFormFileCollection images)
        {
            if (images.Count > maxImagesCount)
                ModelState.AddModelError(nameof(advert.Images), "Нельзя загружать больше 10 изображений");
            if (!ModelState.IsValid)
                return View(advert);

            _advertRepository.SaveAdvert(advert);

            // Сохранение изображений в отдельную папку и добавление их путей в БД
            advert.Images = SaveImages(advert, images);

            //Добавление актуальной даты публикации
            advert.PublicationDate = DateTime.Now;

            // Кто создал объявление
            User currentUser = await GetCurrentUser();
            advert.User = currentUser;
            currentUser.Adverts.Add(advert);

            _advertRepository.SaveAdvert(advert);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteAdvert(Guid id)
        {
            Advert advertToDelete = _advertRepository.GetAdvertById(id);

            //Удаление изображений с сервера
            if(advertToDelete.Images.Length > 0)
                DeleteAllImages(advertToDelete.Images);

            //Удаление записи в БД
            _advertRepository.DeleteAdvert(advertToDelete);

            return RedirectToAction("Index", "Home");
        }

        private string[] SaveImages(Advert advert, IFormFileCollection images)
        {
            Directory.CreateDirectory($"{_appEnvironment.WebRootPath}/usersFiles/advertsImages/{advert.Id}");
            List<string> imagesPaths = new List<string>();
            Guid i = Guid.NewGuid();
            foreach (var image in images)
            {
                using (var fileStream = new FileStream($"{_appEnvironment.WebRootPath}/usersFiles/advertsImages/{advert.Id}/image{i}.png", FileMode.Create, FileAccess.Write))
                {
                    image.CopyTo(fileStream);
                    imagesPaths.Add($"/usersFiles/advertsImages/{advert.Id}/image{i}.png");
                }
                i = Guid.NewGuid();
            }

            return imagesPaths.ToArray();
        }
        
        private void DeleteAllImages(string[] pathsToImages)
        {
            string directoryName = Path.GetDirectoryName(_appEnvironment.WebRootPath + pathsToImages[0]);
            try
            {
                Directory.Delete(directoryName, true);
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                throw new Exception(dirNotFound.Message);
            }
        }

        private async Task<User> GetCurrentUser()
        {
            ClaimsPrincipal currentUser = User;
            return await _userManager.GetUserAsync(currentUser);
        }
    }
}
