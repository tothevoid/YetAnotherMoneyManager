using MoneyManager.Common;
using System;

namespace MoneyManager.DAL.Entities
{
    public class Fund : BaseEntity
    {
        public string Name { get; set; }

        public double Balance { get; set; }
    }
}
