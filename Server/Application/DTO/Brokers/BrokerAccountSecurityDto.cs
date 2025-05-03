using System;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO.Brokers
{
    public class BrokerAccountSecurityDTO: BaseEntity
    {
        public BrokerAccountDTO BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public SecurityDTO Security { get; set; }

        public Guid SecurityId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
