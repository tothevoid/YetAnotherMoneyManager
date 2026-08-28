using Audex.WebApi.Models.Common;
using System;

namespace Audex.WebApi.Models.Brokers
{
    public class GetAllDividendsPaymentsQuery: BasePageableQuery
    {
        public Guid? BrokerAccountId { get; set; }
    }
}
