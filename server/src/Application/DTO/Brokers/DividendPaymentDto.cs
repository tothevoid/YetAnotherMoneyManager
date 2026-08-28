using Audex.Infrastructure.Entities.Brokers;
using Audex.Infrastructure.Entities.Securities;
using Audex.Shared.Entities;
using System;
using Audex.Application.DTO.Securities;

namespace Audex.Application.DTO.Brokers
{
    public class DividendPaymentDto : BaseEntity
    {
        public BrokerAccountDto BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public DividendDto Dividend { get; set; }

        public Guid DividendId { get; set; }

        public int SecuritiesQuantity { get; set; }

        public decimal Tax { get; set; }

        public DateOnly ReceivedAt { get; set; }
    }
}
