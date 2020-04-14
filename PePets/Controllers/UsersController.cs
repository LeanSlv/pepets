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
        private readonly UserRepository _userReposittory;
        private readonly IWebHostEnvironment _appEnvironment;

        public UsersController(UserRepository userRepository, IWebHostEnvironment appEnvironment)
        {
            _userReposittory = userRepository;
            _appEnvironment = appEnvironment;
        }

        public IActionResult UserProfile()
        {
            User currentUser = _userReposittory.GetCurrentUser(User);
            return View(currentUser);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(string id)
        {
            User user = await _userReposittory.GetUserById(id);
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
                User user = await _userReposittory.GetUserById(model.Id);
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

                    var result = await _userReposittory.SaveUser(user);
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
            User user = await _userReposittory.GetUserById(id);
            if (user != null)
            {
                var result = await _userReposittory.Delete(user);
            }
            return RedirectToAction("Index", "Roles");
        }
    }
}