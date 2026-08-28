using System;
using MoneyManager.Infrastructure.Entities.Notifications;
using MoneyManager.Shared.Entities;

namespace MoneyManager.WebApi.Models.Notifications
{
    public class NotificationModel : BaseEntity
    {
        public Guid UserProfileId { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public NotificationSeverity Severity { get; set; }

        public string ActionUrl { get; set; }

        public string Category { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ReadAt { get; set; }
    }
}
