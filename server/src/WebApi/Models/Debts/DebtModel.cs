using System;
using Audex.Shared.Entities;
using Audex.WebApi.Models.Currencies;
using System.Collections.Generic;

namespace Audex.WebApi.Models.Debts
{
    public class DebtModel: BaseEntity
    {
        public string Name { get; set; }

        public CurrencyModel Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public decimal Amount { get; set; }

        public DateOnly Date { get; set; }

        public List<DebtTagModel> DebtTags { get; set; } = new List<DebtTagModel>();
    }
}
