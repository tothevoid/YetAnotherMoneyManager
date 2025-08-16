using MoneyManager.Shared.Entities;

namespace MoneyManager.WebApi.Models.Currencies
{
    public class CurrencyModel: BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        public decimal Rate { get; set; }
    }
}
