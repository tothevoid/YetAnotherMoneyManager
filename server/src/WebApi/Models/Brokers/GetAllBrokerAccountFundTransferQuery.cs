using MoneyManager.WebApi.Models.Common;
using System;

namespace MoneyManager.WebApi.Models.Brokers
{
    public class GetAllBrokerAccountFundTransferQuery: BasePageableQuery
    {
        public Guid? BrokerAccountId { get; set; }
    }
}
