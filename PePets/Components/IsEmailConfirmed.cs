using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using PePets.Repositories;
using System.Threading.Tasks;

namespace PePets.Components
{
    public class IsEmailConfirmed : ViewComponent
    {
        private readonly IUserRepository _userRepository;

        public IsEmailConfirmed(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userName)
        {
            if(userName == null)
                return View("IsEmailConfirmed", true);

            User user = await _userRepository.GetByNameAsync(userName);
            if(user == null)
                return View("IsEmailConfirmed", true);

            bool isEmailConfirmed = await _userRepository.IsEmailConfirmedAsync(user);
            return View("IsEmailConfirmed", isEmailConfirmed);
        }
    }
}
