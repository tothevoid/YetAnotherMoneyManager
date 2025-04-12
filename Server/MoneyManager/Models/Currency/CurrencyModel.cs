using MoneyManager.Shared;

namespace MoneyManager.WebApi.Models.Currency
{
    public class CurrencyModel: BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}
