using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PePets.Services;
using PePets.Repositories;

namespace PePets.Controllers
{
    /// <summary>
    /// Контроллер для управления объявлениями.
    /// </summary>
    public class PostsController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly int maxImagesCount;
        private readonly SearchService _searchService;

        public PostsController(IPostRepository postRepository, IUserRepository userRepository, IWebHostEnvironment appEnvironment, SearchService searchService)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _appEnvironment = appEnvironment;
            _searchService = searchService;
            maxImagesCount = 10;
        }

        /// <summary>
        /// Метод подгружает объявление для его подробного просмотра.
        /// </summary>
        /// <param name="id">Идентификатор объявления, которое необходимо просмотреть.</param>
        /// <returns>Представление определенного объявления.</returns>
        [HttpGet]
        public async Task<IActionResult> ReviewPost(Guid id)
        {
            Post post = await _postRepository.GetByIdAsync(id);
            await _postRepository.AddViewAsync(post);
            return View(post);
        }

        /// <summary>
        /// Метод подгружает страницу для создания объявления.
        /// </summary>
        /// <returns>Представление создания нового объявления.</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreatePost()
        {
            return View(new Post());
        }

        /// <summary>
        /// Метод создает новое объявление.
        /// </summary>
        /// <param name="post">Модель объявлния, которое нужно создать.</param>
        /// <param name="phoneNumberSwitch">Информация со свитча, нужно ли указывать телефон из профиля пользователя.</param>
        /// <param name="images">Коллекция фотографий, которые нужно добавить к объявлению.</param>
        /// <returns>
        /// При удачном создании объявления редирект на главную страницу, при неудачном - представление создания
        /// объявления с ошибками, возникшими при создании объявления.
        /// </returns>
        [HttpPost]
        [RequestSizeLimit(31457280)] // 30 Мб
        public async Task<IActionResult> CreatePost(Post post, string phoneNumberSwitch, IFormFileCollection images)
        {
            User currentUser = _userRepository.GetCurrentUser(User);

            // Если включена опция вставки номера в объявление из профиля пользователя.
            if (phoneNumberSwitch == "on")
                post.PhoneNumber = currentUser.PhoneNumber;

            // Корректно ли объявление.
            if (!CheckPost(post, phoneNumberSwitch, images.Count))
                return View(post);

            // Создаем запись в БД для получения идентификатора поста, чтобы сформировать названия фотографий.
            await _postRepository.CreateAsync(post);

            // Сохранение изображений в отдельную папку и добавление их путей в БД.
            post.Images = SaveImages(post.Id, images);

            //Добавление актуальной даты публикации.
            post.PublicationDate = DateTime.Now;

            // Кто создал объявление.
            await _userRepository.AddPostAsync(currentUser, post);

            // Обновляем запись объявления в БД.
            await _postRepository.UpdateAsync(post);

            // Индексирование данных.
            await _searchService.IndexPost(post);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Метод подгружает страницу редактирования определенного объявления.
        /// </summary>
        /// <param name="id">Идентификатор объявления, которое нужно отредактировать.</param>
        /// <returns>Представление редактирования определенного объявления.</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditPost(Guid id)
        {
            Post post = await _postRepository.GetByIdAsync(id);
            if (User.Identity.Name != post.User.UserName)
                return NotFound();

            return View("CreatePost", post);
        }

        /// <summary>
        /// Метод редактирования определенного объявление.
        /// </summary>
        /// <param name="post">Модель объявлния, которое нужно отредактировать.</param>
        /// <param name="phoneNumberSwitch">Информация со свитча, нужно ли указывать телефон из профиля пользователя.</param>
        /// <param name="images">Коллекция фотографий, которые нужно добавить к объявлению.</param>
        /// <returns>
        /// При удачном редактировании объявления редирект на главную страницу, при неудачном - представление
        /// редактирования объявления с ошибками, возникшими при редактировании.
        /// </returns>
        [HttpPost]
        [RequestSizeLimit(31457280)] // 30 Мб
        public async Task<IActionResult> EditPost(Post post, string phoneNumberSwitch, IFormFileCollection images)
        {
            User currentUser = _userRepository.GetCurrentUser(User);

            // Если включена опция вставки номера в объявление из профиля пользователя.
            if (phoneNumberSwitch == "on")
                post.PhoneNumber = currentUser.PhoneNumber;

            // Корректно ли объявление.
            if (!CheckPost(post, phoneNumberSwitch, images.Count))
                return View(post);

            // Сохранение изображений в отдельную папку и добавление их путей в БД.
            post.Images = SaveImages(post.Id, images);

            // Обноваление записи объявления в БД.
            await _postRepository.UpdateAsync(post);

            // Индексирование данных.
            await _searchService.IndexPost(post);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Метод удаляет определенное объявление.
        /// </summary>
        /// <param name="id">Идентификатор объявления, которое нужно удалить.</param>
        /// <returns>Редирект на главную страницу.</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            Post postToDelete = await _postRepository.GetByIdAsync(id);

            // Удаление изображений с сервера.
            if(postToDelete.Images.Length > 0)
                DeleteAllImages(postToDelete.Images);

            // Удаление записи в БД.
            await _postRepository.DeleteAsync(postToDelete);

            // Удаление индекса
            await _searchService.DeleteIndexAsync(id.ToString());

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Метод добавляет или удаляет объявление в избранное.
        /// </summary>
        /// <param name="id">Идентификатор объявления.</param>
        /// <returns>Если объявление удалено из избранного - false, если добавлено - true.</returns>
        [HttpGet]
        public async Task<bool> Like(Guid id)
        {
            Post post = await _postRepository.GetByIdAsync(id);
            User currentUser = _userRepository.GetCurrentUser(User);

            // Если объявление уже добавленно в избранное,
            if (_userRepository.IsFavoritePost(User, id))
            {
                // то удаляем его,
                await _postRepository.UnlikeAsync(post);
                await _userRepository.DeleteFavoritePostAsync(currentUser, post);
                return false;
            }
            else
            {
                // иначе добавляем в избранное.
                await _postRepository.LikeAsync(post);
                await _userRepository.AddFavoritePostAsync(currentUser, post);
                return true;
            }  
        }

        private bool CheckPost(Post post, string phoneNumberSwitch, int imagesCount)
        {
            if (phoneNumberSwitch == "on")
            {
                if (string.IsNullOrEmpty(post.PhoneNumber))
                    ModelState.AddModelError(nameof(post.PhoneNumber),
                        "В вашем профиле не указан номер телефона, заполните поле самостоятельно");
            }

            if (string.IsNullOrEmpty(post.PhoneNumber) || post.PhoneNumber.Length < 12)
                ModelState.AddModelError(nameof(post.PhoneNumber), "Укажите номер телефона");

            if (imagesCount > maxImagesCount)
                ModelState.AddModelError(nameof(post.Images), "Нельзя загружать больше 10 изображений");

            return ModelState.IsValid;
        }

        private string[] SaveImages(Guid postId, IFormFileCollection images)
        {
            Directory.CreateDirectory($"{_appEnvironment.WebRootPath}/usersFiles/postsImages/{postId}");
            List<string> imagesPaths = new List<string>();
            Guid imageId = Guid.NewGuid();
            foreach (var image in images)
            {
                using (var fileStream = new FileStream($"{_appEnvironment.WebRootPath}/usersFiles/postsImages/{postId}/image{imageId}.png", FileMode.Create, FileAccess.Write))
                {
                    image.CopyTo(fileStream);
                    imagesPaths.Add($"/usersFiles/postsImages/{postId}/image{imageId}.png");
                }
                imageId = Guid.NewGuid();
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
    }
}
