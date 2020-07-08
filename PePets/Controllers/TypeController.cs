using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using PePets.Repositories;

namespace PePets.Controllers
{
    public class TypeController : Controller
    {
        private readonly ITypeRepository _typeRepository;
        public TypeController(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public IActionResult Index() => View();

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