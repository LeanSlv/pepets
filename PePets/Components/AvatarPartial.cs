using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using PePets.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PePets.Components
{
    /// <summary>
    /// Компонент для отображения частичного представления аватарки.
    /// </summary>
    public class AvatarPartial : ViewComponent
    {
        private readonly IUserRepository _userRepository;

        public AvatarPartial(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Метод подгружает представление аватарки пользователя.
        /// </summary>
        /// <param name="userClaims">Удтверждения текущего пользователя.</param>
        /// <returns>Представление аватарки пользователя.</returns>
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
