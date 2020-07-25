using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using PePets.Repositories;

namespace PePets.Controllers
{
    /// <summary>
    /// Контроллер для управления типами животных.
    /// </summary>
    public class TypeController : Controller
    {
        private readonly ITypeRepository _typeRepository;
        public TypeController(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public IActionResult Index() => View();

        /// <summary>
        /// Метод создает новый тип животного с определенным названием.
        /// </summary>
        /// <param name="typeName">Название нового типа животного.</param>
        /// <returns>
        /// При удачном создании редирект на страницу панели администрирования, при неудачном - возвращает
        /// частичное представление с ошибками создания типа.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Create(string typeName)
        {
            if (!string.IsNullOrEmpty(typeName))
            {
                if (await _typeRepository.GetByNameAsync(typeName) == null)
                {
                    var type = new TypeOfPet { Type = typeName };
                    await _typeRepository.CreateAsync(type);

                    return RedirectToAction("Index", "Roles");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Такой вид уже есть в списке");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Поле не может быть пустым");
            }

            return PartialView(typeName);
        }

        /// <summary>
        /// Метод удаляет определенный тип животного.
        /// </summary>
        /// <param name="id">Идентификатор типа животного, которое нужно удалить.</param>
        /// <returns>Редирект на панель администрирования.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var type = await _typeRepository.GetByIdAsync(id);
            if(type != null)
                await _typeRepository.DeleteAsync(type);

            return RedirectToAction("Index", "Roles");
        }
    }
}