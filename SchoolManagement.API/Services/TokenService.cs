using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.API.Models.Entities;

namespace SchoolManagement.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public TokenService(IConfiguration config, UserManager<User> userManager)
        {
            _configuration = config;
            _userManager = userManager;
        }

        public async Task<string> GenerateJwtTokenForUser(User user)
        {
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT Key is not set in the configuration.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.UserName ?? string.Empty),
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: _configuration["Jwt:Expires"] == null
                    ? DateTime.Now.AddMinutes(30)
                    : DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:Expires"])),
                signingCredentials: credentials);

            user.LastSuccessfulLogin = DateTime.Now;

            await _userManager.UpdateAsync(user);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}