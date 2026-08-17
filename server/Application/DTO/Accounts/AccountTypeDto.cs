using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO.Accounts
{
    public class AccountTypeDto : BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}