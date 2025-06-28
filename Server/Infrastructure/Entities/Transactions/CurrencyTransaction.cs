using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Transactions
{
    public class CurrencyTransaction: BaseEntity
    {
        public Account SourceAccount { get; set; }

        public Guid SourceAccountId { get; set; }

        public Account DestinationAccount { get; set; }

        public Guid DestinationAccountId { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }

        public DateOnly Date { get; set; }
    }
}
