using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Shared.Entities;
using MoneyManager.WebApi.Models.Accounts;

namespace MoneyManager.WebApi.Models.Transactions
{
    public class CurrencyTransactionModel: BaseEntity
    {
        public AccountModel SourceAccount { get; set; }

        public Guid SourceAccountId { get; set; }

        public AccountModel DestinationAccount { get; set; }

        public Guid DestinationAccountId { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }

        public DateOnly Date { get; set; }
    }
}
