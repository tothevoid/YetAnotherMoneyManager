using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.User
{
    public class UserProfile: BaseEntity
    {
        public Guid CurrencyId { get; set; }

        public Currency Currency { get; set; }

        public string LanguageCode { get; set; }
    }
}
