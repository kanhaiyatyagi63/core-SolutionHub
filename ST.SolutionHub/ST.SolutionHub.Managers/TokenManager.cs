using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ST.SolutionHub.DataLayer;
using ST.SolutionHub.DataLayer.Abstracts;
using ST.SolutionHub.DataLayer.Entities;
using ST.SolutionHub.Entities.AuthenticationModels;
using ST.SolutionHub.Managers.Abstracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ST.SolutionHub.Managers
{
    public class TokenManager : ITokenManager
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public TokenManager(
            IConfiguration configuration,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<RefreshToken> GetRefreshToken(string token, string ipAddress)
        {
            return await _unitOfWork.RefreshTokenRepositry.GetAsync(x => !x.IsDeleted &&
                x.Token == token &&
                x.ExpiresOn > DateTime.UtcNow &&
                x.IPAddress == ipAddress
            );
        }

        public async Task<AuthToken> GenerateToken(ApplicationUser user, IEnumerable<string> roles, string ipAddress, string refreshToken = null)
        {

            //Generate taken based on claims

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };
            //Add Roles

            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Jwt:Secret"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:TokenExpiryDurationInMinutes")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _configuration.GetValue<string>("Jwt:Audience"),
                Issuer = _configuration.GetValue<string>("Jwt:Issuer"),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            if (string.IsNullOrEmpty(refreshToken))
            {
                refreshToken = await GenerateRefreshToken(user.Id, ipAddress);
            }
            else
            {
                refreshToken = await UpdateRefreshToken(user.Id, ipAddress, refreshToken);
            }

            return new AuthToken()
            {
                JwtToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken
            };
        }

        public string GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidIssuer = _configuration.GetValue<string>("Jwt:Issuer"),
                ValidAudience = _configuration.GetValue<string>("Jwt:Audience"),
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Jwt:Secret"))),
                ValidateIssuer = true,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var claim = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid");
            return claim?.Value;
        }

        public async Task RevokeToken(string refreshToken, string ipAddress)
        {
            var token = await _unitOfWork.RefreshTokenRepositry.GetAsync(x => !x.IsDeleted &&
                  x.Token == refreshToken &&
                  x.IPAddress == ipAddress
            );

            if (token != null)
            {
                token.ExpiresOn = DateTime.UtcNow;
                _unitOfWork.RefreshTokenRepositry.Update(token);
                await _unitOfWork.CommitAsync();
            }
        }

        private async Task<string> UpdateRefreshToken(string userId, string ipAddress, string refreshToken)
        {
            var token = await _unitOfWork.RefreshTokenRepositry.GetAsync(x => !x.IsDeleted &&
                  x.Token == refreshToken &&
                  x.IPAddress == ipAddress &&
                  x.UserId == userId
            );

            if (token != null)
            {
                token.ExpiresOn = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:RefreshTokenExpiryDurationInMinutes"));

                _unitOfWork.RefreshTokenRepositry.Update(token);
                await _unitOfWork.CommitAsync();
            }
            return refreshToken;
        }

        private async Task<string> GenerateRefreshToken(string userId, string ipAddress)
        {
            var token = new RefreshToken()
            {
                Token = GetSecureRandomNumber(),
                ExpiresOn = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:RefreshTokenExpiryDurationInMinutes")),
                UserId = userId,
                IPAddress = ipAddress
            };

            await _unitOfWork.RefreshTokenRepositry.InsertAsync(token);
            await _unitOfWork.CommitAsync();
            return token.Token;
        }

        private string GetSecureRandomNumber()
        {
            var randomNumber = new byte[40];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
