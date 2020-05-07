using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Controllers
{
    public class BreedController : Controller
    {
        private readonly BreedRepository _breedRepository;
        private readonly TypeRepository _typeRepository;

        public BreedController(BreedRepository breedRepository, TypeRepository typeRepository) 
        {
            _breedRepository = breedRepository;
            _typeRepository = typeRepository;
        }

        public IActionResult Index() => View();

        public IActionResult Create(Guid typeId, string breedName)
        {
            if (!string.IsNullOrEmpty(breedName))
            {
                var type = _typeRepository.FindTypeById(typeId);
                if (type != null)
                {
                    var breed = new BreedOfPet { Type = type, Breed = breedName };
                    bool res = _breedRepository.SaveBreed(breed);
                    if (res)
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

        public IActionResult Delete (Guid id)
        {
            var breed = _breedRepository.FindBreedById(id);
            if (breed != null)
                _breedRepository.DeleteBreed(breed);

            return RedirectToAction("Index", "Roles");
        }
    }
}
