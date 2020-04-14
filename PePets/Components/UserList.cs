using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Components
{
    public class UserList : ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public UserList(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("UserList", _userManager.Users.ToList());
        }
    }
}
