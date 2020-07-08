using Microsoft.AspNetCore.Identity;
using PePets.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PePets.Repositories
{
    public interface IUserRepository : IDisposable
    {
        Task<IdentityResult> CreateWithoutPasswordAsync(User user);
        Task<IdentityResult> CreateWithPasswordAsync(User user, string password);
        Task<IdentityResult> UpdateAsync(User user);
        Task<IdentityResult> DeleteAsync(User user);
        IEnumerable<User> GetAll();
        Task<User> GetByIdAsync(string userId);
        Task<User> GetByNameAsync(string name);
        User GetCurrentUser(ClaimsPrincipal currentUserClaims);
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<bool> IsEmailConfirmedAsync(User user);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task AddPostAsync(User user, Post post);
        Task AddFavoritePostAsync(User user, Post post);
        Task DeleteFavoritePostAsync(User user, Post post);
        bool IsFavoritePost(ClaimsPrincipal userClaims, Guid postId);
        Task<List<User>> GetAlreadyRatedUsersAsync(User user);
        Task<bool> CanCurrentUserRateAsync(ClaimsPrincipal currentUserClaims, User user);
        Task RateUserAsync(int rating, User user, User currentUser);
    }
}
