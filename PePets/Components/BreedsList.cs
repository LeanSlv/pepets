using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Components
{
    public class BreedsList : ViewComponent
    {
        private readonly BreedRepository _breedRepository;

        public BreedsList(BreedRepository breedRepository) 
        {
            _breedRepository = breedRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return View("BreedsList", new List<BreedOfPet>());
                        
            return View("BreedsList", _breedRepository.GetAllBreedsOfType(typeName).ToList());
        }
    }
}
