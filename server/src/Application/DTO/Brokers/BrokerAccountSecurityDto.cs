using System;
using Audex.Application.DTO.Securities;
using Audex.Shared.Entities;

namespace Audex.Application.DTO.Brokers
{
    public class BrokerAccountSecurityDto: BaseEntity
    {
        public BrokerAccountDto BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public SecurityDto Security { get; set; }

        public Guid SecurityId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal SoldPrice { get; set; }

        public int SoldQuantity { get; set; }
    }
}
