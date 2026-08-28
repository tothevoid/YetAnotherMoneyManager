using Audex.Shared.Entities;

namespace Audex.Application.DTO.Accounts
{
    public class AccountTypeDto : BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}