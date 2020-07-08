using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using PePets.Repositories;
using System;
using System.Threading.Tasks;

namespace PePets.Controllers
{
    public class BreedController : Controller
    {
        private readonly IBreedRepository _breedRepository;
        private readonly ITypeRepository _typeRepository;

        public BreedController(IBreedRepository breedRepository, ITypeRepository typeRepository) 
        {
            _breedRepository = breedRepository;
            _typeRepository = typeRepository;
        }

        public IActionResult Index() => View();

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

        public async Task<IActionResult> Delete (Guid id)
        {
            var breed = await _breedRepository.GetByIdAsync(id);
            if (breed != null)
                await _breedRepository.DeleteAsync(breed);

            return RedirectToAction("Index", "Roles");
        }

        [HttpGet]
        public IActionResult LoadBreedsViewComponent(string typeName)
        {
            return ViewComponent("BreedsList", typeName);
        }
    }
}
