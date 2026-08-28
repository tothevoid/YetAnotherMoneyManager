using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audex.Application.DTO.Accounts;
using Audex.Infrastructure.Entities.Accounts;
using Audex.Shared.Entities;

namespace Audex.Application.DTO.Transactions
{
    public class CurrencyTransactionDto: BaseEntity
    {
        public string Name { get; set; }

        public AccountDto SourceAccount { get; set; }

        public Guid SourceAccountId { get; set; }

        public AccountDto DestinationAccount { get; set; }

        public Guid DestinationAccountId { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }

        public DateOnly Date { get; set; }
    }
}
