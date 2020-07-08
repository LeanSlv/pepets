using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using PePets.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PePets.Components
{
    public class AvatarPartial : ViewComponent
    {
        private readonly IUserRepository _userRepository;

        public AvatarPartial(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(ClaimsPrincipal userClaims)
        {
            User user = _userRepository.GetCurrentUser(userClaims);
            if (user == null)
                return View("AvatarPartial", "/img/user.png");

            if (string.IsNullOrEmpty(user.Avatar))
                return View("AvatarPartial", "/img/user.png");

            return View("AvatarPartial", user.Avatar);
        }
    }
}
