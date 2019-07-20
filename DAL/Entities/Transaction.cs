using MoneyManager.Common;
using System;

namespace MoneyManager.DAL.Entities
{
    public class Transaction : BaseEntity
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public int MoneyQuantity { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }
    }
}
