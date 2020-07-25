using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using PePets.Repositories;
using System;
using System.Threading.Tasks;

namespace PePets.Controllers
{
    /// <summary>
    /// Контроллер для управления породами животных.
    /// </summary>
    public class BreedController : Controller
    {
        private readonly IBreedRepository _breedRepository;
        private readonly ITypeRepository _typeRepository;

        public BreedController(IBreedRepository breedRepository, ITypeRepository typeRepository) 
        {
            _breedRepository = breedRepository;
            _typeRepository = typeRepository;
        }

        public IActionResult Index() 
        {
            return View();
        }

        /// <summary>
        /// Метод создает новую породу для определенного типа животного.
        /// </summary>
        /// <param name="typeId">Идентификатор типа животного, для которого создается новая порода.</param>
        /// <param name="breedName">Название новой породы.</param>
        /// <returns>
        /// При удачном создании редирект на страницу панели администрирования, при неудачном - возвращает
        /// частичное представление с ошибками создания породы.
        /// </returns>
        public async Task<IActionResult> Create(Guid typeId, string breedName)
        {
            if (!string.IsNullOrEmpty(breedName))
            {
                var type = await _typeRepository.GetByIdAsync(typeId);
                if (type != null)
                {
                    var breed = new BreedOfPet { Type = type, Breed = breedName };
                    await _breedRepository.CreateAsync(breed);

                    return RedirectToAction("Index", "Roles");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Такая порода уже есть в списке");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Поле не может быть пустым");
            }

            return PartialView(breedName);
        }

        /// <summary>
        /// Метод удаляет определенную породу из списка.
        /// </summary>
        /// <param name="id">Идентификатор породы, которую нужно удалить.</param>
        /// <returns>Редирект на страницу панели администрирования.</returns>
        public async Task<IActionResult> Delete (Guid id)
        {
            var breed = await _breedRepository.GetByIdAsync(id);
            if (breed != null)
                await _breedRepository.DeleteAsync(breed);

            return RedirectToAction("Index", "Roles");
        }

        /// <summary>
        /// Метод загружает компонент списка пород для определенного типа животного.
        /// </summary>
        /// <param name="typeName">Название типа животного, для которого нужно загрузить список пород.</param>
        /// <returns>Компонент списка пород для определенного типа животного.</returns>
        [HttpGet]
        public IActionResult LoadBreedsViewComponent(string typeName)
        {
            return ViewComponent("BreedsList", typeName);
        }
    }
}
