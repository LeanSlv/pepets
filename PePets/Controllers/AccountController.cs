using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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
        public IActionResult Login()
        {
            return PartialView();
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
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
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
                    ModelState.AddModelError(nameof(model.Password), "Неправильный логин и (или) пароль");
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
                var result = await _userRepository.SaveUser(user, model.NewPassword);
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
                    string message = GenerateMessageBody(user.FirstName, callbackUrl);

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
            {
                return RedirectToAction("Index", "Home");
            }
            else
                return View("Error");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string providerName, string returnUrl = null)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalLoginCallback",
                    new { returnUrl = returnUrl })
            };

            return new ChallengeResult(providerName, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            // Получаем куки после внешней авторизации
            var info = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            var externalUser = info.Principal;
            if (externalUser == null)
                throw new Exception("External authentication error");

            // Получаем нужные утверждения с основной информацией о пользователе
            var claims = externalUser.Claims.ToList();

            Claim userEmail = claims.Find(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
            Claim userFirstName = claims.Find(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");
            Claim userSecondName = claims.Find(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname");
            Claim userPicture = claims.Find(x => x.Type == "picture");

            User user = await _userRepository.GetUserByEmailAsync(userEmail.Value);
            IdentityResult saveResult;
            if (user == null) // Если пользователь новый, то создаем и регистрируем его
            {
                user = new User 
                { 
                    Email = userEmail.Value, 
                    UserName = userEmail.Value, 
                    FirstName = userFirstName.Value,
                    SecondName = userSecondName.Value,
                    EmailConfirmed = true,
                    Avatar = userPicture.Value,
                    RegistrationDate = DateTime.Now
                };                  
            }
            else // Иначе обновляем его основную информацию на основе полученных от соц. сети
            {
                user.FirstName = userFirstName.Value;
                user.SecondName = userSecondName.Value;
                user.EmailConfirmed = true;
                user.Avatar = userPicture.Value;
            }

            saveResult = await _userRepository.SaveUser(user);
            if(saveResult.Succeeded)
                await _signInManager.SignInAsync(user, false);
            // проверяем, принадлежит ли URL приложению
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }

        private string GenerateMessageBody(string userName, string callbackUrl)
        {
            return
                "<div>" +
                    "<div style = 'padding: 0.5em; margin: 0 auto; width: 50%; font-size: 1.2rem; font-family: Arial, Helvetica, sans-serif;'>" +
                        $"<div style = 'margin-bottom: 0.7em;' > Здравствуйте {userName},</div>" +
                        "<div style = 'margin-bottom: 1.5em;' > Для того, чтобы продолжить регистрацию на сайте PePets.ru, пожалуйста, подтвердите ваш email адрес:</div>" +
                        $"<a style = 'display: flex; padding: 1.5em; background-color: #587fcc; color: white; justify-content: center; text-decoration: none; border-radius: 25px;' href = '{callbackUrl}' > Подтверить email адрес</a>" +
                     "</div>" +
                "</div>";
        }
    }
}