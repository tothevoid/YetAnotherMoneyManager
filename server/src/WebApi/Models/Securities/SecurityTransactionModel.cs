using System;
using Audex.Shared.Entities;
using Audex.WebApi.Models.Brokers;

namespace Audex.WebApi.Models.Securities
{
    public class SecurityTransactionModel: BaseEntity
    {
        public SecurityModel Security { get; set; }

        public Guid SecurityId { get; set; }

        public BrokerAccountModel BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public decimal BrokerCommission { get; set; }

        public decimal StockExchangeCommission { get; set; }

        public decimal Tax { get; set; }

        public bool IsSell { get; set; }
    }
}
