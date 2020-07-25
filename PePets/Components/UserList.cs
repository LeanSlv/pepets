using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Components
{
    /// <summary>
    /// Компонент для работы со списком пользователей.
    /// </summary>
    public class UserList : ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public UserList(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Метод подгружает список пользователей.
        /// </summary>
        /// <returns>Представление списка пользователей.</returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("UserList", _userManager.Users.ToList());
        }
    }
}
