using System;

namespace MoneyManager.WebApi.Models.Accounts
{
    public class AccountTransferModel
    {
        public Guid From { get; set; }

        public Guid To { get; set; }

        public decimal Balance { get; set; }

        public decimal Fee { get; set; }
    }
}
