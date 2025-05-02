using System;

namespace MoneyManager.WebApi.Models.Brokers
{
    public class BrokerAccountSecurityRequestModel
    {
        public int RecordsQuantity { get; set; }

        public int PageIndex { get; set; }

        public Guid BrokerAccountId { get; set; }
    }
}
