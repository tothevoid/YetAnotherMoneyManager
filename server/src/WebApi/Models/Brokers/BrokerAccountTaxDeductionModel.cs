using Audex.Application.DTO.Brokers;
using System;
using System.ComponentModel.DataAnnotations;
using Audex.Shared.Entities;

namespace Audex.WebApi.Models.Brokers
{
    public class BrokerAccountTaxDeductionModel: BaseEntity
    {
        public string Name { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateApplied { get; set; }

        public BrokerAccountModel BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }
    }
}