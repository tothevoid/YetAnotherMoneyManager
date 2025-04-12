using System;
using MoneyManager.BLL.DTO;
using MoneyManager.Common;

namespace BLL.DTO
{
    public class BrokerAccountSecurityDto: BaseEntity
    {
        public BrokerAccountDto BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public SecurityDto Security { get; set; }

        public Guid SecurityId { get; set; }

        public int Quantity { get; set; }

        public decimal InitialPrice { get; set; }

        public decimal CurrentPrice { get; set; }
    }
}
