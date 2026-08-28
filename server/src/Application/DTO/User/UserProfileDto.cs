using System;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO.User
{
    public class UserProfileDto : BaseEntity
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public Guid CurrencyId { get; set; }

        public CurrencyDto Currency { get; set; }

        public string LanguageCode { get; set; }
    }
}
