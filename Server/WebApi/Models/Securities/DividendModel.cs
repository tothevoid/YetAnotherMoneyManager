using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Shared.Entities;
using System;

namespace MoneyManager.WebApi.Models.Securities
{
    public class DividendModel : BaseEntity
    {
        public Security Security { get; set; }

        public Guid SecurityId { get; set; }

        public DateOnly DeclarationDate { get; set; }

        public DateOnly SnapshotDate { get; set; }

        public DateOnly PaymentDate { get; set; }

        public decimal Amount { get; set; }
    }
}
