using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Model.Server
{
    public class DepositsRangeDto
    {
        public DateOnly From { get; set; }

        public DateOnly To { get; set; }
    }
}
