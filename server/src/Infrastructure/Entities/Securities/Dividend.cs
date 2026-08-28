using Audex.Infrastructure.Entities.Brokers;
using Audex.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audex.Infrastructure.Entities.Securities
{
    public class Dividend : BaseEntity
    {
        public Security Security { get; set; }

        public Guid SecurityId { get; set; }

        public DateOnly DeclarationDate { get; set; }

        public DateOnly SnapshotDate { get; set; }

        public decimal Amount { get; set; }

        public ICollection<DividendPayment> DividendPayments { get; set; }
    }
}
