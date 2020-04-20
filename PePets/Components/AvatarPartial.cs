using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PePets.Components
{
    public class AvatarPartial : ViewComponent
    {
        private readonly UserRepository _userRepository;

        public AvatarPartial(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(ClaimsPrincipal userClaims)
        {
            User user = _userRepository.GetCurrentUser(userClaims);
            if (user == null)
                return View();

            if (string.IsNullOrEmpty(user.Avatar))
                return View("AvatarPartial", "/img/user.png");

            return View("AvatarPartial", user.Avatar);
        }
    }
}
