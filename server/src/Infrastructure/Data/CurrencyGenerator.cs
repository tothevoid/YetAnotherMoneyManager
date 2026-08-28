using System;
using Audex.Infrastructure.Constants;
using Audex.Infrastructure.Entities.Currencies;
using Audex.Infrastructure.Interfaces.Utilitary;

namespace Audex.Infrastructure.Data
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
