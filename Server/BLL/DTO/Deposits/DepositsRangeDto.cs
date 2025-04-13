using System;

namespace MoneyManager.Application.DTO.Deposits
{
    public class DepositsRangeDto
    {
        public DateOnly From { get; set; }

        public DateOnly To { get; set; }
    }
}
