using Audex.Infrastructure.Entities.Brokers;
using Audex.Shared.Entities;
using System.Collections.Generic;

namespace Audex.Infrastructure.Entities.Securities
{
    public class SecurityType: BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Security> Securities { get; set; }
    }
}
