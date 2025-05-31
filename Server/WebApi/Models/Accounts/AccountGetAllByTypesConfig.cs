using System;

namespace MoneyManager.WebApi.Models.Accounts
{
    public class AccountGetAllByTypesConfig
    {
        public Guid[] TypesIds { get; set; } = Array.Empty<Guid>();

        public bool OnlyActive { get; set; } = true;
    }
}
