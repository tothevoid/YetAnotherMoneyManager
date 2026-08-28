using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audex.Application.DTO.Currencies;
using Audex.Shared.Entities;

namespace Audex.Application.DTO.Debts
{
    public class DebtDto: BaseEntity
    {
        public string Name { get; set; }

        public CurrencyDto Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public decimal Amount { get; set; }

        public DateOnly Date { get; set; }

        public List<DebtTagDto> DebtTags { get; set; } = new List<DebtTagDto>();
    }
}
