using Microsoft.AspNetCore.Mvc;
using PePets.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace PePets.Components
{
    public class TypesList : ViewComponent
    {
        private readonly ITypeRepository _typeRepository;

        public TypesList(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("TypesList", _typeRepository.GetAll().ToList());
        }
    }
}
