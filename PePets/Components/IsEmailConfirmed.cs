using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using System.Threading.Tasks;

namespace PePets.Components
{
    public class IsEmailConfirmed : ViewComponent
    {
        private readonly UserRepository _userRepository;

        public IsEmailConfirmed(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userName)
        {
            if(userName == null)
                return View("IsEmailConfirmed", true);

            User user = await _userRepository.GetUserByEmailAsync(userName);
            if(user == null)
                return View("IsEmailConfirmed", true);

            bool isEmailConfirmed = await _userRepository.IsEmailConfirmedAsync(user);
            return View("IsEmailConfirmed", isEmailConfirmed);
        }
    }
}
