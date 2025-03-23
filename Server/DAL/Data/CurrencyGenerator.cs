using System;
using DAL.Interfaces.Utilitary;
using MoneyManager.DAL.Entities;

namespace MoneyManager.Data
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
