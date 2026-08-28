using System;
using Audex.Shared.Entities;
using Audex.WebApi.Models.Securities;

namespace Audex.WebApi.Models.Brokers
{
    public class BrokerAccountSecurityModel: BaseEntity
    {
        public BrokerAccountModel BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public SecurityModel Security { get; set; }

        public Guid SecurityId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal SoldPrice { get; set; }

        public int SoldQuantity { get; set; }
    }
}
