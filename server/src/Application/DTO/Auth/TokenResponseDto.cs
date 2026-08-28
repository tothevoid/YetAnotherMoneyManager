#nullable enable
using System;

namespace MoneyManager.Application.DTO.Auth
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }
    }
}
