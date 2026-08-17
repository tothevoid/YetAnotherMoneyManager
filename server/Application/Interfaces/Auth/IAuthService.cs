#nullable enable
using System;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Auth;

namespace MoneyManager.Application.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(string username, string password, string? ipAddress = null, string? userAgent = null);

        Task<TokenResponseDto> RefreshTokenAsync(string refreshToken, string? ipAddress = null, string? userAgent = null);

        Task<bool> RevokeTokenAsync(string refreshToken);

        Task<bool> RevokeAllUserTokensAsync(Guid userProfileId);

        Task<bool> ChangePasswordAsync(string userName, string currentPassword, string newPassword);
    }
}
