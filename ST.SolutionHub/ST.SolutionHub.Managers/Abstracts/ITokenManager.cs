using ST.SolutionHub.DataLayer;
using ST.SolutionHub.DataLayer.Entities;
using ST.SolutionHub.Entities.AuthenticationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ST.SolutionHub.Managers.Abstracts
{
    public interface ITokenManager
    {
        string GetUserIdFromToken(string token);
        Task<AuthToken> GenerateToken(ApplicationUser user, IEnumerable<string> roles, string ipAddress, string refreshToken = null);
        Task<RefreshToken> GetRefreshToken(string token, string ipAddress);
        Task RevokeToken(string refreshToken, string ipAddress);
    }
}
