using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Currencies
{
    public class Currency : BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}