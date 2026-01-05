using System;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO.Brokers
{
    public class BrokerAccountTaxDeductionDto: BaseEntity
    {
        public string Name { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateApplied { get; set; }

        public BrokerAccountDTO BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }
    }
}