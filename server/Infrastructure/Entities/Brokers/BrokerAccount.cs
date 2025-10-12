using System;
using System.Collections.Generic;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Brokers
{
    public class BrokerAccount: BaseEntity
    {
        public string Name { get; set; }

        public BrokerAccountType Type { get; set; }

        public Guid TypeId { get; set; }

        public Currency Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public Broker Broker { get; set; }

        public Guid BrokerId { get; set; }

        public decimal MainCurrencyAmount { get; set; }

        public ICollection<BrokerAccountSecurity> BrokerAccountSecurities { get; set; }

        public ICollection<SecurityTransaction> SecurityTransactions { get; set; }

        public ICollection<DividendPayment> DividendPayments { get; set; }

        public ICollection<BrokerAccountFundsTransfer> BrokerAccountFundsTransfers { get; set; }
    }
}
