using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Transactions
{
    public class TransactionType: BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}
