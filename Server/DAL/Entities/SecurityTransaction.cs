using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Common;
using MoneyManager.DAL.Entities;

namespace MoneyManager.DAL.Entities
{
    public class SecurityTransaction: BaseEntity
    {
        public Security Security { get; set; }

        public Guid SecurityId { get; set; }

        public BrokerAccount BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public DateOnly Date { get; set; }

        public decimal Commission { get; set; }

        public decimal Tax { get; set; }
    }
}
