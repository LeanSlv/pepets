using Microsoft.AspNetCore.Mvc;
using PePets.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Components
{
    /// <summary>
    /// Компонент для работы со списком типов животных.
    /// </summary>
    public class TypesList : ViewComponent
    {
        private readonly ITypeRepository _typeRepository;

        public TypesList(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        /// <summary>
        /// Метод подгружает список типов животных.
        /// </summary>
        /// <returns>Представление списка типов животных.</returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("TypesList", _typeRepository.GetAll().ToList());
        }
    }
}
