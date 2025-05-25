using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.Infrastructure.Configurations.Accounts;

namespace MoneyManager.Infrastructure.Configurations.Debts
{
    public class DebtConfiguration : IEntityTypeConfiguration<Debt>
    {
        public void Configure(EntityTypeBuilder<Debt> debtConfiguration)
        {
            debtConfiguration.HasOne(x => x.Currency);
        }
    }
}