using System;

namespace MoneyManager.Infrastructure.Constants
{
    public static class AccountTypeConstants
    {
        public static Guid Cash = new("a08f5553-379e-4294-a2e5-75e88219433c");
        public static Guid DebitCard = new("cda2ce07-551e-48cf-988d-270c0d022866");
        public static Guid CreditCard = new("6ea1867f-c067-412c-b443-8b9bc2467202");
        public static Guid DepositAccount = new("b9dcea63-49f4-47bf-ae7b-eb596479ba57");
    }
}
