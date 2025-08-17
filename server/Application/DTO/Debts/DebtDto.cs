using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO.Debts
{
    public class DebtDto: BaseEntity
    {
        public string Name { get; set; }

        public CurrencyDTO Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public decimal Amount { get; set; }

        public DateOnly Date { get; set; }
    }
}
