using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
            return _context.Users.Include(x => x.Adverts).ThenInclude(x => x.PetDescription)
                .Include(x => x.FavoriteAdverts).ThenInclude(x => x.PetDescription)
                .SingleOrDefault(x => x.UserName == currentUserClaims.Identity.Name);
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _context.Users.Include(i => i.Adverts).ThenInclude(ti => ti.PetDescription).SingleOrDefaultAsync(s => s.Id == userId);
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

        public async Task AddAdvert(ClaimsPrincipal claims, Advert advert)
        {
            User user = GetCurrentUser(claims);
            user.Adverts.Add(advert);
            await _userManager.UpdateAsync(user);
        }

        public async Task AddFavoriteAdvert(ClaimsPrincipal claims, Advert advert)
        {
            User user = GetCurrentUser(claims);
            user.FavoriteAdverts.Add(advert);
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteFavoriteAdvert(ClaimsPrincipal claims, Advert advert)
        {
            User user = GetCurrentUser(claims);
            user.FavoriteAdverts.Remove(advert);
            await _userManager.UpdateAsync(user);
        }

        public bool IsFavoriteAdvert(ClaimsPrincipal claims, Guid id)
        {
            User user = GetCurrentUser(claims);
            List<Advert> favoriteAdverts = user.FavoriteAdverts;
            foreach(Advert favoriteAdvert in favoriteAdverts)
            {
                if (favoriteAdvert.Id == id)
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
