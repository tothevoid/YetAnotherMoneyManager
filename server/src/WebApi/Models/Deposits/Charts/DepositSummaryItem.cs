using System;

namespace Audex.WebApi.Models.Deposits.Charts
{
    public class DepositSummaryItem
    {
        public Guid DepositId { get; set; }

        public string Name { get; set; }

        public decimal TotalValue { get; set; }
    }
}
