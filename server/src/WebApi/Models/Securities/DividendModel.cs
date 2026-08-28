using Audex.Infrastructure.Entities.Securities;
using Audex.Shared.Entities;
using System;

namespace Audex.WebApi.Models.Securities
{
    public class DividendModel : BaseEntity
    {
        public SecurityModel Security { get; set; }

        public Guid SecurityId { get; set; }

        public DateOnly DeclarationDate { get; set; }

        public DateOnly SnapshotDate { get; set; }

        public decimal Amount { get; set; }
    }
}
