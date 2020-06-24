using System;
using Microsoft.AspNetCore.Mvc;
using PePets.Models;

namespace PePets.Controllers
{
    public class TypeController : Controller
    {
        private readonly TypeRepository _typeRepository;
        public TypeController(TypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Create(string typeName)
        {
            if (!string.IsNullOrEmpty(typeName))
            {
                if (_typeRepository.FindTypeByName(typeName) == null)
                {
                    var type = new TypeOfPet { Type = typeName };
                    bool res = _typeRepository.SaveType(type);
                    if (res)
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
        public IActionResult Delete(Guid id)
        {
            var type = _typeRepository.FindTypeById(id);
            if(type != null)
                _typeRepository.DeleteType(type);

            return RedirectToAction("Index", "Roles");
        }
    }
}