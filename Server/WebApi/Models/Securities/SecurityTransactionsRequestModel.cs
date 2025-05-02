using System;

namespace MoneyManager.WebApi.Models.Securities
{
    public class SecurityTransactionsRequestModel
    {
        public int RecordsQuantity { get; set; }

        public int PageIndex { get; set; }

        public Guid BrokerAccountId { get; set; }
    }
}
