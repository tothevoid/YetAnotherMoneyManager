using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Accounts
{
    public class AccountType : BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}