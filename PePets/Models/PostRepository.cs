using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace PePets.Models
{
    public class PostRepository
    {
        private readonly PePetsDbContext _context;

        public PostRepository(PePetsDbContext context)
        {
            _context = context;
        }

        public IQueryable<Post> GetPosts()
        {
            return _context.Posts.Include(i => i.PetDescription).Include(i => i.User);
        }

        public Post GetPostById(Guid id)
        {
            return _context.Posts.Include(p => p.PetDescription).Include(u => u.User).Single(x => x.Id == id);
        }

        public Guid SavePost(Post post)
        {       
            if (post.Id == default)
            {
                // Если объявления не существует, то добавляем её
                _context.Entry(post).State = EntityState.Added;
                _context.Entry(post.PetDescription).State = EntityState.Added;
            } 
            else
            {
                // Иначе обновляем        
                _context.Entry(post).State = EntityState.Modified;
                _context.Entry(post.PetDescription).State = EntityState.Modified;
            }

            _context.SaveChanges();

            return post.Id;
        }

        public void DeletePost(Post post)
        {
            _context.Remove(post);
            _context.SaveChanges();
        }

        public void LikePost(Post post)
        {
            post.NumberOfLikes += 1;
            _context.Entry(post).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void UnlikePost(Post post)
        {
            post.NumberOfLikes -= 1;
            _context.Entry(post).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void addViewToPost(Post post)
        {
            post.Views += 1;
            _context.Entry(post).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
