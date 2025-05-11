using MoneyManager.Infrastructure.Constants;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Interfaces.Utilitary;

namespace MoneyManager.Infrastructure.Data
{
    public class SecurityTypeGenerator : IDataGenerator<SecurityType>
    {
        public SecurityType[] Generate()
        {
            return new SecurityType[]
            {
                new() { Id = SecurityTypeConstants.Stock, Name = "Stock"},
                new() { Id = SecurityTypeConstants.Bond, Name = "Bond"},
                new() { Id = SecurityTypeConstants.InvestmentFundUnit, Name = "Investment fund unit"},
                new() { Id = SecurityTypeConstants.Currency, Name = "Currency"}
            };
        }
    }
}