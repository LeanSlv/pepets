using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Components
{
    public class RoleList : ViewComponent
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleList(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("RoleList", _roleManager.Roles.ToList());
        }
    }
}
