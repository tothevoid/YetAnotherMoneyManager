using MoneyManager.Application.DTO.Brokers;
using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyManager.WebApi.Models.Brokers
{
    public class BrokerAccountTaxDeductionModel
    {
        public string Name { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateApplied { get; set; }

        public BrokerAccountModel BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }
    }
}