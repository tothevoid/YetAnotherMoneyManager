using MoneyManager.Shared.Entities;
using System.Collections.Generic;

namespace MoneyManager.Infrastructure.Entities.Transactions
{
    public class TransactionType: BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        public string IconKey { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
