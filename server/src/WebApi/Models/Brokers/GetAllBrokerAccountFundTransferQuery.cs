using Audex.WebApi.Models.Common;
using System;

namespace Audex.WebApi.Models.Brokers
{
    public class GetAllBrokerAccountFundTransferQuery: BasePageableQuery
    {
        public Guid? BrokerAccountId { get; set; }
    }
}
