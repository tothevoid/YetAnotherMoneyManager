using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Common;
using MoneyManager.DAL.Entities;

namespace MoneyManager.BLL.DTO
{
    public class Security: BaseEntity
    {
        public string Name { get; set; }

        public string Ticker { get; set; }

        public SecurityType Type { get; set; }

        public Guid TypeId { get; set; }

        public Currency Currency { get; set; }

        public Guid CurrencyId { get; set; }
    }
}
