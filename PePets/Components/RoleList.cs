using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Components
{
    /// <summary>
    /// Компонент для работы со списком ролей.
    /// </summary>
    public class RoleList : ViewComponent
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleList(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Метод подгружает список ролей.
        /// </summary>
        /// <returns>Представление списка ролей.</returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("RoleList", _roleManager.Roles.ToList());
        }
    }
}
