using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ST.SolutionHub.DataLayer;
using ST.SolutionHub.Entities.AuthenticationModels;
using ST.SolutionHub.Managers;
using ST.SolutionHub.Managers.Abstracts;
using ST.SolutionHub.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ST.SolutionHub.Controllers
{
    [Route("api/auth")]
    public class AuthenticateController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenManager _tokenManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthenticateController(UserManager<ApplicationUser> userManager,
                                      RoleManager<IdentityRole> roleManager,
                                      IConfiguration configuration,
                                      ITokenManager tokenManager,
                                      SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _tokenManager = tokenManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        [ProducesResponseType(typeof(AuthenticateResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromForm] AuthenticateRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Missing required field");
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized("Invalid username and passowrd!");
            }
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid username and passowrd!");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var token = await _tokenManager.GenerateToken(user, roles, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());
            return Ok(await GetAuthenticateResponseModel(token, user, roles));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("ExternalLogin")]
        [ProducesResponseType(typeof(AuthenticateResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExternalLoginCallback([FromBody] ExternalAuthRequestModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.IdToken))
                    return BadRequest("Authentication Failed. Please contact support team.");


                var result = ApiManager.Get<TokenInfoResponseModel>("https://oauth2.googleapis.com", "tokeninfo", new KeyValuePair<string, string>("id_token", model.IdToken));

                if (!string.IsNullOrWhiteSpace(result.Error))
                    return BadRequest(result.ErrorDescription);
                if (!result.Iss.Contains(_configuration.GetValue<string>("GoogleOAuthSettings:GoogleClient")) ||
                    !result.Aud.Equals(_configuration.GetValue<string>("GoogleOAuthSettings:GoogleClientId")))
                    return BadRequest("Authentication Failed. Please contact support team.");

                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {

                    ApplicationUser newuser = new ApplicationUser()
                    {
                        Email = model.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = model.Email
                    };
                    var res = await _userManager.CreateAsync(newuser, "Password@1234");
                    if (!res.Succeeded)
                    {
                        return BadRequest("Authentication Failed. Please contact support team.");
                    }
                    user = newuser;
                }
                await _signInManager.SignInAsync(user, isPersistent: false);

                var roles = await _userManager.GetRolesAsync(user);
                var token = await _tokenManager.GenerateToken(user, roles, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());
                return Ok(await GetAuthenticateResponseModel(token, user, roles));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthenticateResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromForm] TokenRequestModel model)
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var refreshToken = await _tokenManager.GetRefreshToken(model.RefreshToken, ipAddress);
            if (refreshToken == null)
            {
                return BadRequest("Invalid Credentials");
            }

            string userId = refreshToken.UserId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("No user found!");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _tokenManager.GenerateToken(user, roles, ipAddress, model.RefreshToken);
            return Ok(await GetAuthenticateResponseModel(token, user, roles));
        }

        [HttpPost()]
        [Route("logout")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout([FromForm] TokenRequestModel model)
        {
            if (string.IsNullOrEmpty(model.RefreshToken))
                return BadRequest(new { message = "RefreshToken is required" });

            //// users can revoke their own tokens and admins can revoke any tokens
            //if (User.GetUserId() != _tokenManager.GetUserIdFromToken(model.Jwt))
            //    return Unauthorized(new { message = "Unauthorized" });

            await _tokenManager.RevokeToken(model.RefreshToken, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());
            return Ok();
        }
        private async Task<AuthenticateResponseModel> GetAuthenticateResponseModel(AuthToken token, ApplicationUser user, IEnumerable<string> roles)
        {
            return new AuthenticateResponseModel()
            {
                Token = token.JwtToken,
                RefreshToken = token.RefreshToken,
                Id = user.Id,
                Name = user.UserName,
                ProfileImgUrl = null,
                Roles = roles,
            };
        }

        #region private
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
        #endregion
    }
}
