using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.Application.Services.User;
using MoneyManager.Infrastructure.Database;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _appConfig;
        private readonly IUserProfileService _userProfileService;
        public AuthService(ApplicationDbContext db, IConfiguration appConfig, IUserProfileService userProfileService)
        {
            _db = db;
            _appConfig = appConfig;
            _userProfileService = userProfileService;
        }

        public async Task<string> LoginAsync(string userName, string password)
        {
            var user = await _userProfileService.GetByAuth(userName, password);
            
            if (user == null)
                throw new ArgumentException(nameof(user));

            var authSection = _appConfig.GetSection("Auth");
            var issuer = authSection.GetSection("Issuer").Value;
            var audience = authSection.GetSection("Audience").Value;
            var secret = authSection.GetSection("Secret").Value;

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(15)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}