using System;
using Audex.Shared.Entities;

namespace Audex.Application.DTO.Brokers
{
    public class BrokerAccountTaxDeductionDto: BaseEntity
    {
        public string Name { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateApplied { get; set; }

        public BrokerAccountDto BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }
    }
}