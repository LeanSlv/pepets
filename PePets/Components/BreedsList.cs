using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using PePets.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Components
{
    /// <summary>
    /// Компонент для работы со списком пород животных.
    /// </summary>
    public class BreedsList : ViewComponent
    {
        private readonly IBreedRepository _breedRepository;

        public BreedsList(IBreedRepository breedRepository) 
        {
            _breedRepository = breedRepository;
        }

        /// <summary>
        /// Метод подгружает список пород для конкретного типа животного.
        /// </summary>
        /// <param name="typeName">Название типа животного, для которого нужно подкгрузить список пород.</param>
        /// <returns>Представление списка пород.</returns>
        public async Task<IViewComponentResult> InvokeAsync(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return View("BreedsList", new List<BreedOfPet>());

            IEnumerable<BreedOfPet> breeds = await _breedRepository.GetAllBreedsOfTypeAsync(typeName);

            return View("BreedsList", breeds.ToList());
        }
    }
}
