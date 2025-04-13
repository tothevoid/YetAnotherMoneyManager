using MoneyManager.Shared.Entities;

namespace MoneyManager.WebApi.Models.Accounts
{
    public class AccountTypeModel : BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}