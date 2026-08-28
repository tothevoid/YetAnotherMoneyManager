using MoneyManager.WebApi.Models.Common;
using System;

namespace MoneyManager.WebApi.Models.Securities
{
    public class GetAllSecuritiesTransactionsQuery: BasePageableQuery
    {
        public Guid? BrokerAccountId { get; set; }
    }
}
