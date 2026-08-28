using System;
using MoneyManager.Infrastructure.Constants;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Infrastructure.Interfaces.Utilitary;

namespace MoneyManager.Infrastructure.Data
{
    public class CurrencyGenerator : IDataGenerator<Currency>
    {
        public Currency[] Generate()
        {
            return new Currency[]
            {
                new() { Id = CurrencyConstants.USD, Active = true, Name = "USD" },
                new() { Id = CurrencyConstants.RUB, Active = true, Name = "RUB" },
                new() { Id = CurrencyConstants.EUR, Active = true, Name = "EUR" },
            };
        }
    }
}
