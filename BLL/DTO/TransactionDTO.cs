using MoneyManager.Common;
using System;

namespace MoneyManager.BLL.DTO
{
    public class TransactionDTO : BaseEntity
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public double MoneyQuantity { get; set; }

        public FundDTO FundSource { get; set; }
    }
}
