using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Common;

namespace MoneyManager.BLL.DTO
{
    public class SecurityDto: BaseEntity
    {
        public string Name { get; set; }

        public string Ticker { get; set; }

        public SecurityTypeDto Type { get; set; }

        public Guid TypeId { get; set; }

        public CurrencyDTO Currency { get; set; }

        public Guid CurrencyId { get; set; }
    }
}
