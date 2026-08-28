using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.User;

namespace MoneyManager.Infrastructure.Configurations.User
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> userProfileConfiguration)
        {
            userProfileConfiguration
                .HasOne(userProfile => userProfile.Currency)
                .WithMany(currency => currency.UserProfiles)
                .HasForeignKey(userProfile => userProfile.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}