using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NaLib_Staff_Management_Lib.Common;
using NaLib_Staff_Management_Lib.Data;
using NaLib_Staff_Management_service.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace NaLib_Staff_Management_service.Controllers
{
    [Route(KnownUrls.Authentification)]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly NaLib_context _Context;
        private readonly NaLibHelpers _lmisHelpers;
        private readonly JwtSettings _jwtSettings;

        public AuthentificationController(NaLib_context context, JwtSettings jwtSettings)
        {
            _Context = context;
            _jwtSettings = jwtSettings;

        }


        /// <summary>
        /// Authenticates a user by verifying their credentials.
        /// </summary>
        /// <param name="credentials">The user's login credentials.</param>
        /// <returns>JWT token, role, and role ID if authentication is successful.</returns>
        /// <response code="200">User authenticated successfully.</response>
        /// <response code="400">Bad request or invalid credentials.</response>
        /// <response code="401">User's account is locked or failed login attempts exceeded.</response>
        /// <response code="403">Initial password expired, user should change password.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Authenticate([FromBody] Credentials credentials)
        {
            if (credentials == null || string.IsNullOrEmpty(credentials.Email) || string.IsNullOrEmpty(credentials.Password))
            {
                return BadRequest("Invalid login request.");
            }

            var user = await _Context.Users
                .Include(u => u.Role) 
                .Where(u => u.Email == credentials.Email)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            if (user.IsAccountLocked)
            {
                return Unauthorized("Your account is locked due to multiple failed login attempts.");
            }

            var hashedPassword = NaLibHelpers.HashPassword(credentials.Password);
            if (user.Password != hashedPassword)
            {
                return Unauthorized("Invalid credentials.");
            }

            if (user.InitialPasswordExpired)
            {
                return Forbid("Please change your initial password.");
            }

            string GenerateJwtToken(string username, Guid roleId, string role)
            {
                var claims = new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("Role", role),
            new Claim("RoleId", roleId.ToString())
        };

                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationInMinutes),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                        SecurityAlgorithms.HmacSha256)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            var token = GenerateJwtToken(user.Email, user.Role.RoleId, user.Role.Name);

            var response = new
            {
                Token = token,
                Role = user.Role.Name,
                RoleId = user.Role.RoleId
            };

            return Ok(new ApiResponse<object> { Data = response });
        }


    }
}
