using System;

namespace MoneyManager.Infrastructure.Constants
{
    public static class AccountTypeConstants
    {
        public static Guid Cash = Guid.Parse("a08f5553-379e-4294-a2e5-75e88219433c");
        public static Guid DebitCard = Guid.Parse("cda2ce07-551e-48cf-988d-270c0d022866");
        public static Guid CreditCard = Guid.Parse("6ea1867f-c067-412c-b443-8b9bc2467202");
        public static Guid DepositAccount = Guid.Parse("b9dcea63-49f4-47bf-ae7b-eb596479ba57");
    }
}
