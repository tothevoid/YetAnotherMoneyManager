using System.Collections.Generic;

namespace MoneyManager.Application.DTO.Securities
{
    public class SecurityHistoryDto
    {
        public IEnumerable<SecurityHistoryValueDto> Values { get; set; } = [];

        public decimal StartPrice { get; set; }

        public decimal EndPrice { get; set; }

        public decimal Diff { get; set; }

        public decimal DiffPercent { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public decimal AvgPrice { get; set; }
    }
}
