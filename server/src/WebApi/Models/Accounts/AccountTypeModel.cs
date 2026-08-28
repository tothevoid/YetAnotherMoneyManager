using Audex.Shared.Entities;

namespace Audex.WebApi.Models.Accounts
{
    public class AccountTypeModel : BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}