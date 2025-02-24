using MoneyManager.Common;
using System;

namespace MoneyManager.DAL.Entities
{
    public class Deposit : BaseEntity
    {
        public string Name { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public short Percentage { get; set; }

        public int InitialAmount { get; set; }
    }
}