using Audex.WebApi.Models.Common;
using System;

namespace Audex.WebApi.Models.Securities
{
    public class GetAllSecuritiesTransactionsQuery: BasePageableQuery
    {
        public Guid? BrokerAccountId { get; set; }
    }
}
