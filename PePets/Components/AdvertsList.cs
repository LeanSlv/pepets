using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PePets.Components
{
    public class AdvertsList : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<Advert> adverts)
        {
            return View("AdvertsList", adverts);
        }
    }
}
