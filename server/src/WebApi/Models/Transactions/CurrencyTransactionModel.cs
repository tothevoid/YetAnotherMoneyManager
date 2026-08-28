using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audex.Infrastructure.Entities.Accounts;
using Audex.Shared.Entities;
using Audex.WebApi.Models.Accounts;

namespace Audex.WebApi.Models.Transactions
{
    public class CurrencyTransactionModel: BaseEntity
    {
        public string Name { get; set; }

        public AccountModel SourceAccount { get; set; }

        public Guid SourceAccountId { get; set; }

        public AccountModel DestinationAccount { get; set; }

        public Guid DestinationAccountId { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }

        public DateOnly Date { get; set; }
    }
}
