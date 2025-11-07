using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.Application.Services.User;
using MoneyManager.Infrastructure.Database;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.User;
using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Auth
{
    // TODO: encrypt passwords

    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<UserProfile> _db;
        private readonly IConfiguration _appConfig;
        private readonly IUserProfileService _userProfileService;
        private readonly IMapper _mapper;

        public AuthService(IMapper mapper, IUnitOfWork uow, IConfiguration appConfig, IUserProfileService userProfileService)
        {
            _uow = uow;
            _db = uow.CreateRepository<UserProfile>();
            _appConfig = appConfig;
            _userProfileService = userProfileService;
            _mapper = mapper;
        }

        public async Task<string> Login(string userName, string password)
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
                // TODO: Fix after implementing refresh tokens
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(120)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public async Task<bool> ChangePassword(string userName, string currentPassword, string newPassword)
        {
            var user = await _userProfileService.GetByAuth(userName, currentPassword);

            if (user == null)
                throw new ArgumentException(nameof(user));

            user.Password = newPassword;

            var mappedUser = _mapper.Map<UserProfile>(user);
            _db.Update(mappedUser);
            await _uow.Commit();
            return true;
        }
    }
}