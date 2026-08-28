using System.Collections.Generic;
using Audex.Shared.Entities;

namespace Audex.Infrastructure.Entities.Debts
{
    public class DebtTag : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string ColorHex { get; set; }

        public ICollection<DebtToDebtTag> DebtAssociations { get; set; } = new List<DebtToDebtTag>();
    }
}
