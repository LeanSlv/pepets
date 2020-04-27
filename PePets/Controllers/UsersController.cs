using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PePets.Models;

namespace PePets.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly AdvertRepository _advertRepository;

        public UsersController(UserRepository userRepository, IWebHostEnvironment appEnvironment, AdvertRepository advertRepository)
        {
            _userRepository = userRepository;
            _appEnvironment = appEnvironment;
            _advertRepository = advertRepository;
        }

        public IActionResult UserProfile()
        {
            User currentUser = _userRepository.GetCurrentUser(User);         
            return View(currentUser);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(string id)
        {
            User user = await _userRepository.GetUserById(id);
            if (user == null)
                return NotFound();


            EditUserProfileViewModel editUserProfileViewModel = new EditUserProfileViewModel
            {
                Id = user.Id,
                FirstName = user.Name,
                SecondName = user.SecondName,
                Age = user.Age,
                Location = user.Location,
                Gender = user.Gender,
                Avatar = user.Avatar
            };
            

            return View(editUserProfileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditUserProfileViewModel model, IFormFile avatar)
        {
            if(ModelState.IsValid)
            {
                User user = await _userRepository.GetUserById(model.Id);
                if(user != null)
                {
                    user.Name = model.FirstName;
                    user.SecondName = model.SecondName;
                    user.Age = model.Age;
                    user.Gender = model.Gender;
                    user.Location = model.Location;

                    if(avatar != null)
                    {
                        // Добавление аватарки
                        using (var fileStream = new FileStream($"{_appEnvironment.WebRootPath}/usersFiles/avatars/avatar_{user.Id}.png", FileMode.Create, FileAccess.Write))
                        {
                            avatar.CopyTo(fileStream);
                            user.Avatar = $"/usersFiles/avatars/avatar_{user.Id}.png";
                        }
                    } 

                    var result = await _userRepository.SaveUser(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("UserProfile");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userRepository.GetUserById(id);
            if (user != null)
            {
                var result = await _userRepository.Delete(user);
            }
            return RedirectToAction("Index", "Roles");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userRepository.GetUserById(id);
            if (user == null)
                return NotFound();

            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id };
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userRepository.GetUserById(model.Id);
                if (user != null)
                {
                    var result = await _userRepository.ChangePassword(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("UserProfile", "Users");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return PartialView(model);
        }
    }
}