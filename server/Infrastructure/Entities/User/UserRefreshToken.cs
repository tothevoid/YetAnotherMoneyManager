#nullable enable
using System;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.User
{
    public class UserRefreshToken : BaseEntity
    {
        public Guid UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; } = null!;

        public string TokenHash { get; set; } = string.Empty;
        public string JwtId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }

        public string? CreatedByIp { get; set; }
        public string? UserAgent { get; set; }

        public Guid? ReplacedByTokenId { get; set; }
        public UserRefreshToken? ReplacedByToken { get; set; }
    }
}
