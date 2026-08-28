using System;
using Audex.Shared.Entities;

namespace Audex.WebApi.Models.Debts
{
    public class DebtTagModel : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string ColorHex { get; set; } = "#3182CE";

        public int UsageCount { get; set; }
    }
}
