using System;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO.Securities
{
    public class SecurityTransactionDTO: BaseEntity
    {
        public SecurityDTO Security { get; set; }

        public Guid SecurityId { get; set; }

        public BrokerAccountDTO BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public decimal Commission { get; set; }

        public decimal Tax { get; set; }
    }
}
