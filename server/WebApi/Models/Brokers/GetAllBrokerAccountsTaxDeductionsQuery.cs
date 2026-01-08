using System;

namespace MoneyManager.WebApi.Models.Brokers
{
    public class GetAllBrokerAccountsTaxDeductionsQuery
    {
        public Guid? BrokerAccountId { get; set; }
    }
}
