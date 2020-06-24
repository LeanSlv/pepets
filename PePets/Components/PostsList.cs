using Microsoft.AspNetCore.Mvc;
using PePets.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PePets.Components
{
    public class PostsList : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<Post> posts)
        {
            return View("PostsList", posts);
        }
    }
}
