using System;

namespace MoneyManager.WebApi.Models.Deposits.Charts
{
    public class DepositPayment
    {
        public Guid DepositId { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }
    }
}
