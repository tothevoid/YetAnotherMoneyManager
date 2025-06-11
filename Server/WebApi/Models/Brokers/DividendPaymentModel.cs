using MoneyManager.Shared.Entities;
using System;
using MoneyManager.WebApi.Models.Securities;

namespace MoneyManager.WebApi.Models.Brokers
{
    public class DividendPaymentModel : BaseEntity
    {
        public BrokerAccountModel BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public DividendModel Dividend { get; set; }

        public Guid DividendId { get; set; }

        public int SecuritiesQuantity { get; set; }

        public decimal Tax { get; set; }
    }
}
