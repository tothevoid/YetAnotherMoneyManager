using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO
{
    public class UserProfileDto: BaseEntity
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public Guid CurrencyId { get; set; }

        public CurrencyDTO Currency { get; set; }

        public string LanguageCode { get; set; }

    }
}
