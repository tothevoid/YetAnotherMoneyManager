using Audex.Infrastructure.Entities.Brokers;
using Audex.Infrastructure.Entities.Currencies;
using Audex.Shared.Entities;
using System;
using System.Collections.Generic;
using Audex.Infrastructure.Entities.Banks;
using Audex.Infrastructure.Entities.Debts;
using Audex.Infrastructure.Entities.Deposits;
using Audex.Infrastructure.Entities.Transactions;

namespace Audex.Infrastructure.Entities.Accounts
{
    public class Account : BaseEntity
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public Currency Currency { get; set; }

        public AccountType AccountType { get; set; }

        public Guid CurrencyId { get; set; }

        public Guid AccountTypeId { get; set; }

        public Bank Bank { get; set; }

        public Guid? BankId { get; set; }

        public DateOnly CreatedOn { get; set; }

        public bool Active { get; set; }

        public ICollection<DebtPayment> DebtPayments { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public ICollection<CurrencyTransaction> SourceCurrencyTransactions { get; set; }

        public ICollection<CurrencyTransaction> DestinationCurrencyTransactions { get; set; }

        public ICollection<BrokerAccountFundsTransfer> BrokerAccountFundsTransfers { get; set; }
    }
}
