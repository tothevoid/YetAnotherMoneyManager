#nullable enable
using MoneyManager.Application.DTO.User;

namespace MoneyManager.Application.DTO.Auth
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public bool PasswordChangeRequired { get; set; }

        public UserProfileDto UserProfile { get; set; } = null!;
    }
}
