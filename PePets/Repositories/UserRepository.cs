using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PePets.Data;
using PePets.Models;

namespace PePets.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly PePetsDbContext _context;
        private bool disposed;

        public UserRepository(UserManager<User> userManager, PePetsDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> CreateWithoutPasswordAsync(User user)
        {
            return await _userManager.CreateAsync(user);
        }

        public async Task<IdentityResult> CreateWithPasswordAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.Include(i => i.Posts).ThenInclude(ti => ti.PetDescription)
                .Include(i => i.AlreadyRatedUsers);
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            return await _context.Users.Include(i => i.Posts).ThenInclude(ti => ti.PetDescription)
                .Include(i => i.AlreadyRatedUsers).SingleOrDefaultAsync(sod => sod.Id == userId);
        }

        public async Task<User> GetByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public User GetCurrentUser(ClaimsPrincipal currentUserClaims)
        {
            return _context.Users.Include(i => i.Posts).ThenInclude(ti => ti.PetDescription)
                .Include(i => i.FavoritePosts).ThenInclude(ti => ti.PetDescription)
                .SingleOrDefault(sod => sod.UserName == currentUserClaims.Identity.Name);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task AddPostAsync(User user, Post post)
        {
            user.Posts.Add(post);
            await _userManager.UpdateAsync(user);
        }

        public async Task AddFavoritePostAsync(User user, Post post)
        {
            user.FavoritePosts.Add(post);
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteFavoritePostAsync(User user, Post post)
        {
            user.FavoritePosts.Remove(post);
            await _userManager.UpdateAsync(user);
        }

        public bool IsFavoritePost(ClaimsPrincipal userClaims, Guid postId)
        {
            User currentUser = GetCurrentUser(userClaims);
            List<Post> favoritePosts = currentUser.FavoritePosts;
            foreach (Post favoritePost in favoritePosts)
            {
                if (favoritePost.Id == postId)
                    return true;
            }

            return false;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<bool> IsEmailConfirmedAsync(User user)
        {
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<List<User>> GetAlreadyRatedUsersAsync(User user)
        {
            User userWithAlreadyRatedUsers = await _context.Users
                .Include(i => i.AlreadyRatedUsers)
                .SingleOrDefaultAsync(sod => sod.Id == user.Id);

            return userWithAlreadyRatedUsers.AlreadyRatedUsers;
        }

        public async Task<bool> CanCurrentUserRateAsync(ClaimsPrincipal currentUserClaims, User user)
        {
            User currentUser = GetCurrentUser(currentUserClaims);
            List<User> alreadyRatedUsers = await GetAlreadyRatedUsersAsync(user);

            foreach (User alreadyRatedUser in alreadyRatedUsers)
            {
                if (alreadyRatedUser.Id == currentUser.Id)
                    return false;
            }

            return true;
        }

        public async Task RateUserAsync(int rating, User user, User currentUser)
        {
            List<User> alreadyRateUsers = await GetAlreadyRatedUsersAsync(user);

            user.Rating = Math.Round(RecountRating(user.Rating, alreadyRateUsers.Count, rating), 2);

            user.AlreadyRatedUsers.Add(currentUser);

            await _userManager.UpdateAsync(user);
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

        private double RecountRating(double oldRating, int countVoters, int newRating)
        {
            return (oldRating * countVoters + newRating) / (countVoters + 1);
        }
    }
}