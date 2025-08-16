using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Shared.Entities;
using System.Collections.Generic;

namespace MoneyManager.Infrastructure.Entities.Securities
{
    public class SecurityType: BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Security> Securities { get; set; }
    }
}
