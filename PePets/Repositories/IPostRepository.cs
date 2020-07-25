using PePets.Models;
using System;
using System.Threading.Tasks;

namespace PePets.Repositories
{
    public interface IPostRepository : ICRUD<Post>, IDisposable
    {
        Task LikeAsync(Post post);
        Task UnlikeAsync(Post post);
        Task AddViewAsync(Post post);
    }
}
