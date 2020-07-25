using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PePets.Components
{
    /// <summary>
    /// Компонент для работы со списком объявлений.
    /// </summary>
    public class PostsList : ViewComponent
    {
        /// <summary>
        /// Метод подгружает список объявлений.
        /// </summary>
        /// <param name="posts">Список объявлений, которые нужно отобразить.</param>
        /// <returns>Представление списка объявлений.</returns>
        public async Task<IViewComponentResult> InvokeAsync(List<Post> posts)
        {
            return View("PostsList", posts);
        }
    }
}
