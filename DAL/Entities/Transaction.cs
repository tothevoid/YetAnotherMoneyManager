using MoneyManager.Common;
using System;

namespace MoneyManager.DAL.Entities
{
    public class Transaction : BaseEntity
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public double MoneyQuantity { get; set; }

        public int Type { get; set; }
    }
}
