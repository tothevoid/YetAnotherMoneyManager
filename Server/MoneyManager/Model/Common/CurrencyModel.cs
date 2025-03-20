using MoneyManager.Common;

namespace MoneyManager.WEB.Model
{
    public class CurrencyModel: BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}
