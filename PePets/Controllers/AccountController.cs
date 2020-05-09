using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using PePets.Services;

namespace PePets.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserRepository userRepository, SignInManager<User> signInManager)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return PartialView(new Login { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.Name,
                    RegistrationDate = DateTime.Today
                };

                // добавляем пользователя в БД
                var result = await _userRepository.SaveUser(user, model.Password);
                if (result.Succeeded)
                {
                    // генерация токена для пользователя
                    string token = await _userRepository.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action
                        (
                            "ConfirmEmail",
                            "Account",
                            new { userId = user.Id, token = token },
                            protocol: HttpContext.Request.Scheme
                        );

                    // Содержимое сообщения
                    string subject = "Подтвердите ваш email aдрес для регистрации на сайте PePets.ru";
                    string message = $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>Подтверить email адрес</a>";

                    // Отправка сообщения пользователю на Email для его подтверждения
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.Email, subject, message);

                    // установка куки
                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return View("Error");

            User user = await _userRepository.GetUserById(userId);
            if (user == null)
                return View("Error");

            var result = await _userRepository.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return View("Error");
        }
    }
}