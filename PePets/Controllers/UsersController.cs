using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using PePets.Repositories;

namespace PePets.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IPostRepository _postRepository;

        public UsersController(IUserRepository userRepository, IWebHostEnvironment appEnvironment, IPostRepository postRepository)
        {
            _userRepository = userRepository;
            _appEnvironment = appEnvironment;
            _postRepository = postRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserProfile(string id)
        {
            User user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            User currentUser = _userRepository.GetCurrentUser(User);
            if (currentUser.Id == id)
                ViewBag.CurrentUser = true;
            else
                ViewBag.CurrentUser = false;

            return View(user);
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditProfile()
        {
            User currentUser = _userRepository.GetCurrentUser(User);
            if (currentUser == null)
                return NotFound();


            return View(currentUser);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangePersonalData(string id)
        {
            User user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            ChangePersonalDataViewModel model = new ChangePersonalDataViewModel { Id = user.Id };
            return PartialView(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePersonalData(ChangePersonalDataViewModel changePersonalData, IFormFile upload_avatar)
        {
            if (ModelState.IsValid)
            {
                User user = await _userRepository.GetByIdAsync(changePersonalData.Id);
                if(user != null)
                {
                    user.FirstName = changePersonalData.FirstName;
                    user.SecondName = changePersonalData.SecondName;

                    if(upload_avatar != null)
                    {
                        // Удаление старой аватарки
                        if (user.Avatar.Contains("/usersFiles/avatars/"))
                        {
                            string directoryName = Path.GetDirectoryName(_appEnvironment.WebRootPath + user.Avatar);                          
                            try
                            {
                                System.IO.File.Delete(directoryName);
                            }
                            catch (DirectoryNotFoundException dirNotFound)
                            {
                                throw new Exception(dirNotFound.Message);
                            }
                        }

                        // Добавление новой аватарки
                        using (var fileStream = new FileStream($"{_appEnvironment.WebRootPath}/usersFiles/avatars/avatar_{user.Id}.png", FileMode.Create, FileAccess.Write))
                        {
                            upload_avatar.CopyTo(fileStream);
                            user.Avatar = $"/usersFiles/avatars/avatar_{user.Id}.png";
                        }
                    }

                    var result = await _userRepository.CreateWithoutPasswordAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("EditProfile");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return PartialView(changePersonalData);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangeContactInfo(string id)
        {
            User user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            ChangeContactInfoViewModel model = new ChangeContactInfoViewModel { Id = user.Id };
            return PartialView(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeContactInfo(ChangeContactInfoViewModel changeContactInfo)
        {
            if(ModelState.IsValid)
            {
                User user = await _userRepository.GetByIdAsync(changeContactInfo.Id);
                if(user != null)
                {
                    user.Location = changeContactInfo.Location;
                    user.PhoneNumber = changeContactInfo.PhoneNumber;
                    user.Gender = changeContactInfo.Gender;
                    user.DateOfBirth = changeContactInfo.DateOfBirth;
                    user.AboutMe = changeContactInfo.AboutMe;

                    var result = await _userRepository.CreateWithoutPasswordAsync(user);
                    if(result.Succeeded)
                    {
                        RedirectToAction("EditProfile");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return PartialView(changeContactInfo);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id };
            return PartialView(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePassword)
        {
            if (ModelState.IsValid)
            {
                User user = await _userRepository.GetByIdAsync(changePassword.Id);
                if (user != null)
                {
                    var result = await _userRepository.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("EditProfile");
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
            return PartialView(changePassword);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                foreach (Post post in user.Posts)
                    await _postRepository.DeleteAsync(post);

                foreach(Post post in user.FavoritePosts)
                    await _postRepository.UnlikeAsync(post);       

                await _userRepository.DeleteAsync(user);
            }

            return RedirectToAction("Index", "Roles");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Rate(int rating, string userId, string returnUrl = null)
        {
            User currentUser = _userRepository.GetCurrentUser(User);
            User user = await _userRepository.GetByIdAsync(userId);

            await _userRepository.RateUserAsync(rating, user, currentUser);

            return Redirect(returnUrl);
        }
    }
}