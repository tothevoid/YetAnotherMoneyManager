using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Shared.Entities;
using System;
using MoneyManager.Application.DTO.Securities;

namespace MoneyManager.Application.DTO.Brokers
{
    public class DividendPaymentDto : BaseEntity
    {
        public BrokerAccountDTO BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public DividendDto Dividend { get; set; }

        public Guid DividendId { get; set; }

        public int SecuritiesQuantity { get; set; }

        public decimal Tax { get; set; }

        public DateOnly ReceivedAt { get; set; }
    }
}
