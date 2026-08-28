using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Audex.Infrastructure.Entities.Notifications;

namespace Audex.Infrastructure.Configurations.Notifications
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(2048);

            builder.Property(n => n.ActionUrl)
                .HasMaxLength(512);

            builder.Property(n => n.Category)
                .IsRequired()
                .HasMaxLength(64);

            builder.HasOne(n => n.UserProfile)
                .WithMany()
                .HasForeignKey(n => n.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
