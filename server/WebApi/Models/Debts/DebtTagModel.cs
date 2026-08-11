using System;
using MoneyManager.Shared.Entities;

namespace MoneyManager.WebApi.Models.Debts
{
    public class DebtTagModel : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string ColorHex { get; set; } = "#3182CE";

        public int UsageCount { get; set; }
    }
}
