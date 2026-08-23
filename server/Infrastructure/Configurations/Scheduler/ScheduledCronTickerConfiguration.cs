using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Scheduler;

namespace MoneyManager.Infrastructure.Configurations.Scheduler
{
    public class ScheduledCronTickerConfiguration : IEntityTypeConfiguration<ScheduledCronTicker>
    {
        public void Configure(EntityTypeBuilder<ScheduledCronTicker> builder)
        {
            builder.HasMany(t => t.Occurrences)
                .WithOne(o => o.CronTicker)
                .HasForeignKey(o => o.CronTickerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
