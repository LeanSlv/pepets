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

namespace PePets.Controllers
{
    public class PostsController : Controller
    {
        private readonly PostRepository _postRepository;
        private readonly UserRepository _userRepository;
        IWebHostEnvironment _appEnvironment;
        private readonly int maxImagesCount;
        private readonly SearchService _searchService;

        public PostsController(PostRepository postRepository, UserRepository userRepository, IWebHostEnvironment appEnvironment, SearchService searchService)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _appEnvironment = appEnvironment;
            _searchService = searchService;
            maxImagesCount = 10;
        }

        public IActionResult PostReview(Guid id)
        {
            Post post = _postRepository.GetPostById(id);
            _postRepository.addViewToPost(post);
            return View(post);
        }

        [Authorize]
        public IActionResult PostEdit(Guid id)
        {
            if (id == default)
                return View(new Post());

            Post post = _postRepository.GetPostById(id);
            if (User.Identity.Name != post.User.UserName)
                return NotFound();

            return View(post);
        }

        [HttpPost]
        [RequestSizeLimit(31457280)] // 30 Мб
        public async Task<IActionResult> PostEdit(Post post, string phoneNumberSwitch, IFormFileCollection images)
        {
            if (phoneNumberSwitch == "on")
            {
                User user = _userRepository.GetCurrentUser(User);
                if (string.IsNullOrEmpty(user.PhoneNumber))
                {
                    ModelState.AddModelError(nameof(post.PhoneNumber), "В вашем профиле не указан номер телефона, заполните поле самостоятельно");
                }
                else
                {
                    post.PhoneNumber = user.PhoneNumber;
                }
            }

            if(string.IsNullOrEmpty(post.PhoneNumber) || post.PhoneNumber.Length < 12)
                ModelState.AddModelError(nameof(post.PhoneNumber), "Укажите номер телефона");

            if (images.Count > maxImagesCount)
                ModelState.AddModelError(nameof(post.Images), "Нельзя загружать больше 10 изображений");

            if (!ModelState.IsValid)
                return View(post);

            _postRepository.SavePost(post);

            // Сохранение изображений в отдельную папку и добавление их путей в БД
            post.Images = SaveImages(post.Id, images);

            //Добавление актуальной даты публикации
            post.PublicationDate = DateTime.Now;

            // Кто создал объявление
            await _userRepository.AddPost(User, post);

            // Сохранение объявления в БД
            _postRepository.SavePost(post);

            // Индексирование данных
            await _searchService.IndexPost(post);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            Post postToDelete = _postRepository.GetPostById(id);

            // Удаление изображений с сервера
            if(postToDelete.Images.Length > 0)
                DeleteAllImages(postToDelete.Images);

            // Удаление записи в БД
            _postRepository.DeletePost(postToDelete);

            // Удаление индекса
            await _searchService.DeleteIndex(id.ToString());

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult LoadBreedsViewComponent(string typeName)
        {
            return ViewComponent("BreedsList", typeName);
        }

        [HttpGet]
        public async Task<bool> Like(Guid id)
        {
            Post post = _postRepository.GetPostById(id);
            User currentUser = _userRepository.GetCurrentUser(User);

            // Если объявление уже добавленно в избранное,
            if (_userRepository.IsFavoritePost(User, id))
            {
                // то удаляем его,
                _postRepository.UnlikePost(post);
                await _userRepository.DeleteFavoritePost(currentUser, post);
                return false;
            }
            else
            {
                // иначе добавляем в избранное.
                _postRepository.LikePost(post);
                await _userRepository.AddFavoritePost(User, post);
                return true;
            }  
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
