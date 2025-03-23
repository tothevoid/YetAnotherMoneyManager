using DAL.Constants;
using DAL.Interfaces.Utilitary;
using MoneyManager.DAL.Entities;
using System;

namespace MoneyManager.Data
{
    public class AccountTypeGenerator: IDataGenerator<AccountType>
    {
        public AccountType[] Generate()
        {
            return new AccountType[]
            {
                new() { Id = AccountTypeConstants.Cash, Active = true, Name = "Cash" },
                new() { Id = AccountTypeConstants.CreditCard, Active = true, Name = "Debit card" },
                new() { Id = AccountTypeConstants.CreditCard, Active = true, Name = "Credit card" },
                new() { Id = AccountTypeConstants.DepositAccount, Active = true, Name = "Deposit account" },
            };
        }
    }
}
