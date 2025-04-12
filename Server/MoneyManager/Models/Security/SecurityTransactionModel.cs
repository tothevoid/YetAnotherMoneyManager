using System;
using MoneyManager.Shared.Entities;
using MoneyManager.WebApi.Models.Broker;

namespace MoneyManager.WebApi.Models.Security
{
    public class SecurityTransactionModel: BaseEntity
    {
        public SecurityModel Security { get; set; }

        public Guid SecurityId { get; set; }

        public BrokerAccountModel BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public DateOnly Date { get; set; }

        public decimal Commission { get; set; }

        public decimal Tax { get; set; }
    }
}
