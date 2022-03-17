using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ST.SolutionHub.DataLayer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ST.SolutionHub.Managers.Abstracts
{
    public interface IApplicationUserManager
    {
        IQueryable<ApplicationUser> Users { get; }
        Task<ApplicationUser> FindByNameAsync(string userName);
        Task<ApplicationUser> FindByUserNameOrEmail(string userNameOrEmail);
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task<IdentityResult> UpdateAsync(ApplicationUser user);
        Task<(bool Succeeded, string ErrorMessage)> RecoverAccount(string userNameOrEmail);
        Task<IList<string>> GetUserRoles(ApplicationUser user);
        Task<bool> IsInRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> DeleteAsync(ApplicationUser user);
        Task<(bool IsValid, string ErrorMessage, ApplicationUser User, IEnumerable<string> Roles)> ValidateUser(string userName, string password, bool isForChangePassword = false);
        Task<(bool IsValid, string ErrorMessage, ApplicationUser User, IEnumerable<string> Roles)> ValidateUser(string userId);
        Task<(bool Succeeded, IEnumerable<string> ErrorMessage)> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
        Task ResetPasswordAsync(ApplicationUser user, string newPassword);
        Task<ApplicationUser> GetUser(string email);
        Task<string> GetProfileImageUrl(string profileImage);
        Task<string> ResendAccountVerificationEmail(string[] emails);
        Task UpdateName();
        Task GenerateSendUnlockAccountLink(string userNameOrEmail);
        Task InsertUserPasswordAsync(string userId, string passwordHash);
        Task<bool> CheckPasswordHistory(ApplicationUser user, string newPassword);
        Task UpdateUserTable();
        Task<(bool IsSuccess, string Message)> AddUserAsync(ApplicationUser user, AppRole role, IFormFile profileImgFile, string[] tags);
    }
}
