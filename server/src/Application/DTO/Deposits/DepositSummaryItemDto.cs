using System;

namespace Audex.Application.DTO.Deposits
{
    public class DepositSummaryItemDto
    {
        public Guid DepositId { get; set; }

        public string Name { get; set; }

        public decimal TotalValue { get; set; }
    }
}
