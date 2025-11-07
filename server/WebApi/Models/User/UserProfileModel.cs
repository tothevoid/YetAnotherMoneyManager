using System;
using MoneyManager.Shared.Entities;
using MoneyManager.WebApi.Models.Currencies;

namespace MoneyManager.WebApi.Models.User
{
    public class UserProfileModel: BaseEntity
    {
        public string UserName { get; set; }

        public Guid CurrencyId { get; set; }

        public CurrencyModel Currency { get; set; }

        public string LanguageCode { get; set; }
    }
}
