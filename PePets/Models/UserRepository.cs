using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PePets.Models
{
    public class UserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly PePetsDbContext _context;

        public UserRepository(UserManager<User> userManager, PePetsDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public User GetCurrentUser(ClaimsPrincipal currentUserClaims)
        {
            return _context.Users.Include(x => x.Posts).ThenInclude(x => x.PetDescription)
                .Include(x => x.FavoritePosts).ThenInclude(x => x.PetDescription)
                .SingleOrDefault(x => x.UserName == currentUserClaims.Identity.Name);
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _context.Users.Include(i => i.Posts).ThenInclude(ti => ti.PetDescription).Include(i => i.AlreadyRatedUsers).SingleOrDefaultAsync(s => s.Id == userId);
        }

        public async Task<User> GetUserByEmailAsync(string email) =>
            await _userManager.FindByNameAsync(email);


        public async Task<IdentityResult> SaveUser(User user, string password = "")
        {
            User currentUser = await _userManager.FindByIdAsync(user.Id);
            if(currentUser == null)
            {
                if (string.IsNullOrEmpty(password))
                    return await _userManager.CreateAsync(user);
                else
                    return await _userManager.CreateAsync(user, password);
            }   
            else
            {
                return await _userManager.UpdateAsync(user);
            }
        }

        public async Task<IdentityResult> Delete(User user) =>
            await _userManager.DeleteAsync(user);

        public async Task<IdentityResult> ChangePassword(User user, string oldPassword, string newPassword) =>
            await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user) => 
            await _userManager.GenerateEmailConfirmationTokenAsync(user);

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token) => 
            await _userManager.ConfirmEmailAsync(user, token);

        public async Task<bool> IsEmailConfirmedAsync(User user) => 
            await _userManager.IsEmailConfirmedAsync(user);

        public async Task<bool> CheckPasswordAsync(User user, string password) =>
            await _userManager.CheckPasswordAsync(user, password);

        public async Task AddPost(ClaimsPrincipal claims, Post post)
        {
            User user = GetCurrentUser(claims);
            user.Posts.Add(post);
            await _userManager.UpdateAsync(user);
        }

        public async Task AddFavoritePost(ClaimsPrincipal claims, Post post)
        {
            User user = GetCurrentUser(claims);
            user.FavoritePosts.Add(post);
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteFavoritePost(User user, Post post)
        {
            user.FavoritePosts.Remove(post);
            await _userManager.UpdateAsync(user);
        }

        public bool IsFavoritePost(ClaimsPrincipal claims, Guid id)
        {
            User user = GetCurrentUser(claims);
            List<Post> favoritePosts = user.FavoritePosts;
            foreach(Post favoritePost in favoritePosts)
            {
                if (favoritePost.Id == id)
                    return true;
            }

            return false;
        }

        public async Task<List<User>> GetAlreadyRatedUsers(User user)
        {
            User userWithAlreadyRatedUsers = await _context.Users.Include(i => i.AlreadyRatedUsers).SingleOrDefaultAsync(sor => sor.Id == user.Id);
            
            return userWithAlreadyRatedUsers.AlreadyRatedUsers;
        }

        public async Task<bool> CanCurrentUserRate(ClaimsPrincipal currentUserClaims, User user)
        {
            User currentUser = GetCurrentUser(currentUserClaims);
            List<User> alreadyRatedUsers = await GetAlreadyRatedUsers(user);

            foreach (User alreadyRatedUser in alreadyRatedUsers)
            {
                if (alreadyRatedUser.Id == currentUser.Id)
                    return false;
            }

            return true;
        }

        public async Task RateUser(int rating, User user, User currentUser)
        {
            List<User> alreadyRateUsers = await GetAlreadyRatedUsers(user);

            user.Rating = Math.Round(RecountRating(user.Rating, alreadyRateUsers.Count, rating), 2);

            user.AlreadyRatedUsers.Add(currentUser);

            await _userManager.UpdateAsync(user);
        }

        private double RecountRating(double oldRating, int countVoters, int newRating)
        {
            return (oldRating * countVoters + newRating) / (countVoters + 1);
        }
    }
}
