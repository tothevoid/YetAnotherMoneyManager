using Audex.Shared.Entities;
using System.Collections.Generic;

namespace Audex.Infrastructure.Entities.Brokers
{
    public class Broker: BaseEntity
    {
        public string Name { get; set; }

        public ICollection<BrokerAccount> BrokerAccounts { get; set; }
    }
}
