using Audex.Infrastructure.Constants;
using Audex.Infrastructure.Entities.Securities;
using Audex.Infrastructure.Interfaces.Utilitary;

namespace Audex.Infrastructure.Data
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
                new() { Id = SecurityTypeConstants.Currency, Name = "Currency"},
                new() { Id = SecurityTypeConstants.PreciousMetal, Name = "Precious metal"}
            };
        }
    }
}