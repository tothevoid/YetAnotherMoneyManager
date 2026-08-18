#nullable enable
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoneyManager.Application.DTO;
using MoneyManager.Application.DTO.Auth;
using MoneyManager.Application.Interfaces.Auth;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.Application.Mappings;
using MoneyManager.Infrastructure.Entities.User;
using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<UserProfile> _userProfileRepo;
        private readonly IRepository<UserRefreshToken> _refreshTokenRepo;
        private readonly IConfiguration _appConfig;
        private readonly IUserProfileService _userProfileService;
        private readonly ApplicationMapper _mapper;

        public AuthService(
            ApplicationMapper mapper,
            IUnitOfWork uow,
            IConfiguration appConfig,
            IUserProfileService userProfileService)
        {
            _uow = uow;
            _userProfileRepo = uow.CreateRepository<UserProfile>();
            _refreshTokenRepo = uow.CreateRepository<UserRefreshToken>();
            _appConfig = appConfig;
            _userProfileService = userProfileService;
            _mapper = mapper;
        }

        public async Task<LoginResponseDto> LoginAsync(string userName, string password, string? ipAddress = null, string? userAgent = null)
        {
            var user = await _userProfileService.GetByAuthAsync(userName, password);

            if (user == null)
            {
                throw new ArgumentException("Invalid username or password.", nameof(userName));
            }

            var isPasswordEmpty = string.IsNullOrEmpty(password) && string.IsNullOrEmpty(user.Password);

            var (accessToken, rawRefreshToken, refreshTokenEntity) = CreateRefreshTokenEntity(user, ipAddress, userAgent);

            await _refreshTokenRepo.AddAsync(refreshTokenEntity);
            await _uow.CommitAsync();

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = rawRefreshToken,
                PasswordChangeRequired = isPasswordEmpty,
                UserProfile = user
            };
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken, string? ipAddress = null, string? userAgent = null)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new SecurityException("Refresh token is required.");
            }

            var storedToken = await FindTokenByHashAsync(refreshToken);

            if (storedToken == null)
            {
                throw new SecurityException("Invalid refresh token.");
            }

            // Reuse Detection: If an already-used token is presented, compromise is detected.
            if (storedToken.IsUsed)
            {
                var gracePeriodResponse = await TryGetGracePeriodTokenResponseAsync(storedToken);
                if (gracePeriodResponse != null)
                {
                    return gracePeriodResponse;
                }

                await RevokeAllUserTokensAsync(storedToken.UserProfileId);
                throw new SecurityException("Refresh token reuse detected. All sessions revoked.");
            }

            if (storedToken.IsRevoked || storedToken.ExpiresAt <= DateTime.UtcNow)
            {
                throw new SecurityException("Refresh token is expired or revoked.");
            }

            var user = await _userProfileRepo.GetByIdAsync(storedToken.UserProfileId);
            if (user == null)
            {
                throw new SecurityException("User profile not found.");
            }

            var userDto = _mapper.Map(user);

            // Mark old token as used
            storedToken.IsUsed = true;

            // Generate new token pair
            var (newAccessToken, newRawRefreshToken, newRefreshTokenEntity) = CreateRefreshTokenEntity(userDto, ipAddress, userAgent);

            storedToken.ReplacedByTokenId = newRefreshTokenEntity.Id;
            _refreshTokenRepo.Update(storedToken);

            await _refreshTokenRepo.AddAsync(newRefreshTokenEntity);
            await _uow.CommitAsync();

            return new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRawRefreshToken,
                ExpiresAt = newRefreshTokenEntity.ExpiresAt
            };
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return false;
            }

            var storedToken = await FindTokenByHashAsync(refreshToken);

            if (storedToken == null || storedToken.IsRevoked)
            {
                return false;
            }

            storedToken.IsRevoked = true;
            _refreshTokenRepo.Update(storedToken);
            await _uow.CommitAsync();
            return true;
        }

        public async Task<bool> RevokeAllUserTokensAsync(Guid userProfileId)
        {
            var activeTokens = await _refreshTokenRepo.GetAllAsync(token => token.UserProfileId == userProfileId && !token.IsRevoked);
            foreach (var token in activeTokens)
            {
                token.IsRevoked = true;
                _refreshTokenRepo.Update(token);
            }

            await _uow.CommitAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(string userName, string currentPassword, string newPassword)
        {
            var user = await _userProfileService.GetByAuthAsync(userName, currentPassword);

            if (user == null)
            {
                throw new ArgumentException(nameof(user));
            }

            user.Password = newPassword;

            var mappedUser = _mapper.Map(user);
            _userProfileRepo.Update(mappedUser);

            // Revoke all sessions on password change
            await RevokeAllUserTokensAsync(user.Id);

            await _uow.CommitAsync();
            return true;
        }

        private async Task<TokenResponseDto?> TryGetGracePeriodTokenResponseAsync(UserRefreshToken storedToken)
        {
            if (!storedToken.ReplacedByTokenId.HasValue)
            {
                return null;
            }

            var replacementToken = await _refreshTokenRepo.GetByIdAsync(storedToken.ReplacedByTokenId.Value);
            if (replacementToken == null || replacementToken.IsRevoked || replacementToken.CreatedAt < DateTime.UtcNow.AddSeconds(-30))
            {
                return null;
            }

            var activeUser = await _userProfileRepo.GetByIdAsync(storedToken.UserProfileId);
            if (activeUser == null)
            {
                return null;
            }

            var activeUserDto = _mapper.Map(activeUser);
            var (activeAccessToken, _) = GenerateAccessToken(activeUserDto);

            return new TokenResponseDto
            {
                AccessToken = activeAccessToken,
                RefreshToken = string.Empty,
                ExpiresAt = replacementToken.ExpiresAt
            };
        }

        private async Task<UserRefreshToken?> FindTokenByHashAsync(string rawRefreshToken)
        {
            var tokenHash = HashToken(rawRefreshToken);
            var storedTokens = await _refreshTokenRepo.GetAllAsync(token => token.TokenHash == tokenHash);
            return storedTokens.FirstOrDefault();
        }

        private (string accessToken, string rawRefreshToken, UserRefreshToken entity) CreateRefreshTokenEntity(
            UserProfileDto user,
            string? ipAddress,
            string? userAgent)
        {
            var (accessToken, jwtId) = GenerateAccessToken(user);
            var rawRefreshToken = GenerateRefreshToken();
            var tokenHash = HashToken(rawRefreshToken);
            var expiresAt = DateTime.UtcNow.AddDays(30);

            var entity = new UserRefreshToken
            {
                Id = Guid.NewGuid(),
                UserProfileId = user.Id,
                TokenHash = tokenHash,
                JwtId = jwtId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt,
                IsUsed = false,
                IsRevoked = false,
                CreatedByIp = ipAddress,
                UserAgent = userAgent
            };

            return (accessToken, rawRefreshToken, entity);
        }

        private (string token, string jti) GenerateAccessToken(UserProfileDto user)
        {
            var authSection = _appConfig.GetSection("Auth");
            var issuer = authSection.GetSection("Issuer").Value;
            var audience = authSection.GetSection("Audience").Value;
            var secret = authSection.GetSection("Secret").Value;

            var jti = Guid.NewGuid().ToString();

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, jti)
            };

            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!)),
                    SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return (token, jti);
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
        }

        private static string HashToken(string rawToken)
        {
            var bytes = Encoding.UTF8.GetBytes(rawToken);
            var hash = SHA256.HashData(bytes);
            return Convert.ToHexStringLower(hash);
        }
    }
}