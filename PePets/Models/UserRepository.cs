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
            return _context.Users.Include(x => x.Adverts).Include(x => x.FavoriteAdverts)
                .SingleOrDefault(x => x.UserName == currentUserClaims.Identity.Name);
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> SaveUser(User user, string password="")
        {
            if(password != "")
                return await _userManager.CreateAsync(user, password);
            else
                return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> Delete(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> ChangePassword(User user, string oldPassword, string newPassword) =>
            await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
    }
}
