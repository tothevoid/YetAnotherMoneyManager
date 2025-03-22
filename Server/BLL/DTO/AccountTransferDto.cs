using System;

namespace MoneyManager.WEB.Model
{
    public class AccountTransferDto
    {
        public Guid From { get; set; }

        public Guid To { get; set; }

        public double Balance { get; set; }

        public double Fee { get; set; }
    }
}
