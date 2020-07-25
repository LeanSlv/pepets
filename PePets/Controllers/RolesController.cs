using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PePets.Models;

namespace PePets.Controllers
{
    /// <summary>
    /// Контроллер для управления ролями пользователей.
    /// </summary>
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index() => View();

        /// <summary>
        /// Метод создает новую роль.
        /// </summary>
        /// <param name="name">Название новой роли.</param>
        /// <returns>
        /// При удачном создании роли редирект на страницу панели администрирования, при неудачном - представление 
        /// списка ролей с ошибками, возникшими при создании роли.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }

        /// <summary>
        /// Метод удаляет определенную роль.
        /// </summary>
        /// <param name="id">Идентификатор роли, которую нужно удалить.</param>
        /// <returns>Редирект на страницу панели администрирования.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Метод подгружает страницу для редактирования ролей определенного пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя, которому нужно редактировать роли.</param>
        /// <returns>
        /// Если пользователь существует, то возвращает представление редактирования ролей, если нет - страница с
        /// ошибкой, что нужная страница не найдена.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    Name = user.FirstName,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }

        /// <summary>
        /// Метод редактирует роли для определенного пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, которому нужно редактировать роли.</param>
        /// <param name="roles">Список ролей, которое нужно выставить определенному пользователю.</param>
        /// <returns>
        /// Если роли отредактированы удачно, то редирект на панель администрирования, если неудачно - страница с
        /// ошибкой, что нужная страница не найдена.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}