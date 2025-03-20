using MoneyManager.DAL.Entities;

namespace MoneyManager.Data
{
    public class DefaultCurrencies
    {
        public Currency[] GetCurrencies()
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
