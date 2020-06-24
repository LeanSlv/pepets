using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PePets.Services;
using PePets.Models;

namespace PePets.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchService _searchService;
        private readonly PostRepository _postRepository;

        public SearchController(SearchService searchService, PostRepository postRepository)
        {
            _searchService = searchService;
            _postRepository = postRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string input)
        {
            List<Guid> ids = await _searchService.Search(input);

            var posts = new List<Post>();
            foreach (Guid id in ids)
                posts.Add(_postRepository.GetPostById(id));

            return ViewComponent("PostsList", posts);
        }
    }
}