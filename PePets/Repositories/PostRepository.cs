using Microsoft.EntityFrameworkCore;
using PePets.Data;
using PePets.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PePets.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly PePetsDbContext _context;
        private bool disposed;

        public PostRepository(PePetsDbContext context)
        {
            _context = context;
            disposed = false;
        }

        public async Task LikeAsync(Post post)
        {
            post.NumberOfLikes += 1;
            await UpdateAsync(post);
        }

        public async Task UnlikeAsync(Post post)
        {
            post.NumberOfLikes -= 1;
            await UpdateAsync(post);
        }

        public async Task AddViewAsync(Post post)
        {
            post.Views += 1;
            await UpdateAsync(post);
        }

        public async Task CreateAsync(Post post)
        {
            await SaveStateOfPostAsync(post, EntityState.Added);
        }

        public async Task UpdateAsync(Post post)
        {
            await SaveStateOfPostAsync(post, EntityState.Modified);
        }

        public async Task DeleteAsync(Post post)
        {
            _context.Remove(post);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts
                .Include(i => i.PetDescription)
                .Include(i => i.User);
        }

        public async Task<Post> GetByIdAsync(Guid id)
        {
            return await _context.Posts
                .Include(i => i.PetDescription)
                .Include(i => i.User)
                .SingleOrDefaultAsync(sod => sod.Id == id);
        }

        public async Task<Post> GetByNameAsync(string name)
        {
            return await _context.Posts
                .Include(i => i.PetDescription)
                .Include(i => i.User)
                .SingleOrDefaultAsync(sod => sod.Title == name);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async Task SaveStateOfPostAsync(Post post, EntityState state)
        {
            _context.Entry(post).State = state;
            _context.Entry(post.PetDescription).State = state;
            await _context.SaveChangesAsync();
        }
    }
}