using Audex.WebApi.Models.Common;
using System;

namespace Audex.WebApi.Models.Securities
{
    public class GetAllDividendsQuery: BasePageableQuery
    {
        public Guid SecurityId { get; set; }
    }
}
