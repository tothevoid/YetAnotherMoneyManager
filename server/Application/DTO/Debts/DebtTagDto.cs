using System;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO.Debts
{
    public class DebtTagDto : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string ColorHex { get; set; } = "#3182CE";

        public int UsageCount { get; set; }
    }
}
