using MoneyManager.Infrastructure.Constants;
using MoneyManager.Infrastructure.Entities.Account;
using MoneyManager.Infrastructure.Interfaces.Utilitary;

namespace MoneyManager.Infrastructure.Data
{
    public class AccountTypeGenerator: IDataGenerator<AccountType>
    {
        public AccountType[] Generate()
        {
            return new AccountType[]
            {
                new() { Id = AccountTypeConstants.Cash, Active = true, Name = "Cash" },
                new() { Id = AccountTypeConstants.DebitCard, Active = true, Name = "Debit card" },
                new() { Id = AccountTypeConstants.CreditCard, Active = true, Name = "Credit card" },
                new() { Id = AccountTypeConstants.DepositAccount, Active = true, Name = "Deposit account" },
            };
        }
    }
}
