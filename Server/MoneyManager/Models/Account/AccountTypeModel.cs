using MoneyManager.Shared;

namespace MoneyManager.WebApi.Models.Account
{
    public class AccountTypeModel : BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}