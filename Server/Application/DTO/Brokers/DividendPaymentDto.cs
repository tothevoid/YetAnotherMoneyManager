using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Shared.Entities;
using System;

namespace MoneyManager.Application.DTO.Brokers
{
    public class DividendPaymentDto : BaseEntity
    {
        public BrokerAccount BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public Dividend Dividend { get; set; }

        public Guid DividendId { get; set; }

        public int SecuritiesQuantity { get; set; }

        public decimal Tax { get; set; }
    }
}
