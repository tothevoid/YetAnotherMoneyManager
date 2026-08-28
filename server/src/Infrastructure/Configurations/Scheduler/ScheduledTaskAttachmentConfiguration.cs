using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Audex.Infrastructure.Entities.Scheduler;

namespace Audex.Infrastructure.Configurations.Scheduler
{
    public class ScheduledTaskAttachmentConfiguration : IEntityTypeConfiguration<ScheduledTaskAttachment>
    {
        public void Configure(EntityTypeBuilder<ScheduledTaskAttachment> builder)
        {
            builder.Property(a => a.FileName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(a => a.BucketName)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(a => a.StoragePath)
                .IsRequired()
                .HasMaxLength(512);

            builder.Property(a => a.ContentType)
                .IsRequired()
                .HasMaxLength(128);

            builder.HasIndex(a => a.OccurrenceId);
        }
    }
}

