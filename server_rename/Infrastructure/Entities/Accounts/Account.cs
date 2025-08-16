using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Shared.Entities;
using System;
using System.Collections.Generic;
using MoneyManager.Infrastructure.Entities.Debts;
using MoneyManager.Infrastructure.Entities.Deposits;
using MoneyManager.Infrastructure.Entities.Transactions;

namespace MoneyManager.Infrastructure.Entities.Accounts
{
    public class Account : BaseEntity
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public Currency Currency { get; set; }

        public AccountType AccountType { get; set; }

        public Guid CurrencyId { get; set; }

        public Guid AccountTypeId { get; set; }

        public DateOnly CreatedOn { get; set; }

        public bool Active { get; set; }

        public ICollection<DebtPayment> DebtPayments { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public ICollection<CurrencyTransaction> SourceCurrencyTransactions { get; set; }

        public ICollection<CurrencyTransaction> DestinationCurrencyTransactions { get; set; }
    }
}
