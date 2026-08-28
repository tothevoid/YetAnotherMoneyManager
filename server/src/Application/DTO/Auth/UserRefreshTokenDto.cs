#nullable enable
using System;

namespace Audex.Application.DTO.Auth
{
    public class UserRefreshTokenDto
    {
        public Guid Id { get; set; }

        public string? CreatedByIp { get; set; }

        public string? UserAgent { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsCurrent { get; set; }

        public bool IsRevoked { get; set; }

        public bool IsUsed { get; set; }
    }
}
