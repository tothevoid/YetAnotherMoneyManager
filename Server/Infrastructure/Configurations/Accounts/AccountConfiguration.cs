using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Currencies;

namespace MoneyManager.Infrastructure.Configurations.Accounts
{
    public class AccountConfiguration: IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> accountConfiguration)
        {
            accountConfiguration
                .HasOne(account => account.Currency)
                .WithMany(currency => currency.Accounts)
                .HasForeignKey(account => account.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            accountConfiguration
                .HasOne(account => account.AccountType)
                .WithMany(accountType => accountType.Accounts)
                .HasForeignKey(account => account.AccountTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
