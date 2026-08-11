using System.Collections.Generic;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Debts
{
    public class DebtTag : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string ColorHex { get; set; } = "#3182CE";

        public ICollection<DebtToDebtTag> DebtAssociations { get; set; } = new List<DebtToDebtTag>();
    }
}
