using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.User;

namespace MoneyManager.Infrastructure.Configurations.User
{
    public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.HasIndex(token => token.TokenHash)
                .IsUnique();

            builder.HasOne(token => token.UserProfile)
                .WithMany()
                .HasForeignKey(token => token.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(token => token.ReplacedByToken)
                .WithMany()
                .HasForeignKey(token => token.ReplacedByTokenId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
