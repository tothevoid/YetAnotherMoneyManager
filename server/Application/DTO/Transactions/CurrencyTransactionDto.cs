using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO.Transactions
{
    public class CurrencyTransactionDto: BaseEntity
    {
        public string Name { get; set; }

        public AccountDTO SourceAccount { get; set; }

        public Guid SourceAccountId { get; set; }

        public AccountDTO DestinationAccount { get; set; }

        public Guid DestinationAccountId { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }

        public DateOnly Date { get; set; }
    }
}
