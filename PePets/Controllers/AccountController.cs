using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using PePets.Repositories;
using PePets.Services;

namespace PePets.Controllers
{
    /// <summary>
    /// Контроллер для управлением авторизации/регистрации пользователей.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailService _emailService;

        public AccountController(IUserRepository userRepository, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _emailService = new EmailService(configuration["CompanyEmailAuth:Login"], configuration["CompanyEmailAuth:Password"]);
        }

        /// <summary>
        /// Метод для GET запроса авторизации пользователя.
        /// </summary>
        /// <returns>Частичное представление окна авторизации пользователя.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return PartialView();
        }

        /// <summary>
        /// Метод для POST запроса авторизации пользователя, авторизует пользователя на сайте, используя логин и пароль.
        /// </summary>
        /// <param name="model">Модель для авторизации пользователя.</param>
        /// <returns>При удачной авторизации происходит редирект на главную страницу, при неудачной - возвращает 
        /// частичное представление окна авторизации с ошибками авторизации.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Password), "Неправильный логин и (или) пароль");
                }
            }

            return PartialView(model);
        }

        /// <summary>
        /// Метод для GET запроса регистрации пользователя.
        /// </summary>
        /// <returns>Частичное представление окна регистрации пользователя.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return PartialView();
        }

        /// <summary>
        /// Метод для POST запроса регистрации пользователя, регистрирует пользователя на сайте, используя 
        /// имя, логин и пароль.
        /// </summary>
        /// <param name="model">Модель для регистрации пользователя.</param>
        /// <returns>
        /// При удачной регистрации происходит редирект на главную страницу, при неудачной - возвращает 
        /// частичное представление окна авторизации с ошибками регистрации.
        /// </returns>
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

                var result = await _userRepository.CreateWithPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    // Генерация токена для пользователя.
                    string token = await _userRepository.GenerateEmailConfirmationTokenAsync(user);
                    
                    // Формирование ссылки для подтверждения регистрации.
                    string callbackUrl = Url.Action
                        (
                            "ConfirmEmail",
                            "Account",
                            new { userId = user.Id, token = token },
                            protocol: HttpContext.Request.Scheme
                        );

                    // Отправка сообщения пользователю на Email для его подтверждения.
                    await _emailService.SendEmailAsync(user.FirstName, user.Email, callbackUrl);

                    // установка куки.
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

        /// <summary>
        /// Метод для выхода пользователя из аккаунта.
        /// </summary>
        /// <returns>Редирект на главную страницу.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки.
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Метод для подтверждения регистрации новым пользователем. Вызывается, когда пользователь переходит по
        /// сгенерированной ссылке, отправленной на электронную почту.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="token">Сгенерированный токен для данного пользователя.</param>
        /// <returns>При удачном подтверждении редирект на главную страницу, при неудачном - представление с ошибкой.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return View("Error");

            User user = await _userRepository.GetByIdAsync(userId);
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

        /// <summary>
        /// Метод для внешней авторизации OAuth  с помощью социальных сетей.
        /// </summary>
        /// <param name="providerName">Имя провайдера авторизации.</param>
        /// <returns>Окно авторизации определенного провайдера.</returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string providerName)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalLoginCallback")
            };

            return new ChallengeResult(providerName, properties);
        }

        /// <summary>
        /// Метод авторизует пользователя после успешного входа в аккаунт социальной сети через OAuth.
        /// </summary>
        /// <returns>Редирект на главную страницу.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            // Получаем куки после внешней авторизации.
            var info = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            var externalUser = info.Principal;
            if (externalUser == null)
                throw new Exception("External authentication error");

            // Получаем нужные утверждения с основной информацией о пользователе.
            var claims = externalUser.Claims.ToList();

            Claim userEmail = claims.Find(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
            Claim userFirstName = claims.Find(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");
            Claim userSecondName = claims.Find(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname");
            Claim userPicture = claims.Find(x => x.Type == "picture");

            User user = await _userRepository.GetByNameAsync(userEmail.Value);
            IdentityResult saveResult;
            if (user == null) // Если пользователь новый, то создаем и регистрируем его.
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

                saveResult = await _userRepository.CreateWithoutPasswordAsync(user);
            }
            else // Иначе обновляем его основную информацию на основе полученных от соц. сети.
            {
                user.FirstName = userFirstName.Value;
                user.SecondName = userSecondName.Value;
                user.EmailConfirmed = true;
                user.Avatar = userPicture.Value;

                saveResult = await _userRepository.UpdateAsync(user);
            }

            if(saveResult.Succeeded)
                await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }
    }
}