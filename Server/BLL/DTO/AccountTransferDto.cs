using System;

namespace MoneyManager.WEB.Model
{
    public class AccountTransferDto
    {
        public Guid From { get; set; }

        public Guid To { get; set; }

        public decimal Balance { get; set; }

        public decimal Fee { get; set; }
    }
}
