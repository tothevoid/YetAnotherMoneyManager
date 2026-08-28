using MoneyManager.WebApi.Models.Common;
using System;

namespace MoneyManager.WebApi.Models.Securities
{
    public class GetAllDividendsQuery: BasePageableQuery
    {
        public Guid SecurityId { get; set; }
    }
}
