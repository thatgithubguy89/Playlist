using Playlist.Api.Entities.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Playlist.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> CreateToken(AuthenticationRequest request)
        {
            // Get the user
            var user = await _userManager.FindByNameAsync(request.Username);

            // Create the claims and key for the signature
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];

            // Create the signature
            var symmetricSecurityKey = new SymmetricSecurityKey(Convert.FromBase64String(key));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // Use this to add more claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
            };

            // Create the token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Get the user and compare passwords
        public async Task<bool> AuthenticateUser(AuthenticationRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            return user != null && await _userManager.CheckPasswordAsync(user, request.Password);
        }
    }
}
