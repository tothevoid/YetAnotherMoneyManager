using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.DTO.Securities
{
    public class SecurityHistoryValueDto
    {
        public DateTime Date { get; set; }

        public decimal Value { get; set; }
    }
}
