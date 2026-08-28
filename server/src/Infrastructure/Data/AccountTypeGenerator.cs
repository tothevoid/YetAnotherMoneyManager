using Audex.Infrastructure.Constants;
using Audex.Infrastructure.Entities.Accounts;
using Audex.Infrastructure.Interfaces.Utilitary;

namespace Audex.Infrastructure.Data
{
    public class AccountTypeGenerator: IDataGenerator<AccountType>
    {
        public AccountType[] Generate()
        {
            return new AccountType[]
            {
                new() { Id = AccountTypeConstants.Cash, Active = true, Name = "Cash" },
                new() { Id = AccountTypeConstants.DebitCard, Active = true, Name = "Debit card" },
                new() { Id = AccountTypeConstants.CreditCard, Active = true, Name = "Credit card" }
            };
        }
    }
}
