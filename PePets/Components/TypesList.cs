using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Components
{
    public class TypesList : ViewComponent
    {
        private readonly TypeRepository _typeRepository;

        public TypesList(TypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("TypesList", _typeRepository.Types.ToList());
        }
    }
}
