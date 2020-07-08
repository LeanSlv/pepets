using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PePets.Models;
using PePets.Repositories;

namespace PePets.Controllers
{
    /// <summary>
    /// Контроллер главной страницы.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostRepository _postRepository;

        public HomeController(ILogger<HomeController> logger, IPostRepository postRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
        }

        /// <summary>
        /// Метод подгружает список всех объявлений.
        /// </summary>
        /// <returns>Представление списка всех объявлений.</returns>
        public IActionResult Index()
        {
            var posts = _postRepository.GetAll();
            return View(posts.ToList());
        }

        /// <summary>
        /// Метод загружает страницу политики конфиденциальности.
        /// </summary>
        /// <returns>Возвращает страницу политики конфиденциальности</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Метод подгружает ошибки, выявленные при работе сайта.
        /// </summary>
        /// <returns>Представление с описанием ошибки.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
