using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Infrastructure.Entities.Securities
{
    public class Dividend : BaseEntity
    {
        public Security Security { get; set; }

        public Guid SecurityId { get; set; }

        public DateOnly DeclarationDate { get; set; }

        public DateOnly SnapshotDate { get; set; }

        public DateOnly PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public ICollection<DividendPayment> DividendPayments { get; set; }
    }
}
