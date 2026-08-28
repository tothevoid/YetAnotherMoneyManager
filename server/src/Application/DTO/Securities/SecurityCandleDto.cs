using System;

namespace MoneyManager.Application.DTO.Securities
{
    public class SecurityCandleDto
    {
        public decimal Open { get; set; }

        public decimal Close { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Value { get; set; }

        public decimal Volume { get; set; }

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }
    }
}
