using MoneyManager.Model.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class PeriodPaymentDTO
    {
        public string Period { get; set; }

        public IEnumerable<DepositPaymentDTO> Payments { get; set; }
    }
}
