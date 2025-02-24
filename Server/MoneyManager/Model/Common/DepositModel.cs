using System;
using MoneyManager.Common;

namespace MoneyManager.Model.Common
{
    public class DepositModel: BaseEntity
    {
        public string Name { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public short Percentage { get; set; }

        public int InitialAmount { get; set; }
    }
}
