#nullable enable
using System;

namespace Audex.Application.DTO.Auth
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }
    }
}
