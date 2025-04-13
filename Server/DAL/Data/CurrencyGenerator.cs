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
                new() { Active = true, Name = "USD" },
                new() { Active = true, Name = "RUB" },
                new() { Active = true, Name = "EUR" },
            };
        }
    }
}
