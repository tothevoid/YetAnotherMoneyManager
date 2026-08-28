using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audex.Shared.Entities;

namespace Audex.Application.DTO.Securities
{
    public class DividendDto: BaseEntity
    {
        public SecurityDto Security { get; set; }

        public Guid SecurityId { get; set; }

        public DateOnly DeclarationDate { get; set; }

        public DateOnly SnapshotDate { get; set; }

        public decimal Amount { get; set; }
    }
}
