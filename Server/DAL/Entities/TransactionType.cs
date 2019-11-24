using MoneyManager.Common;
using System;

namespace MoneyManager.DAL.Entities
{
    public class TransactionType: BaseEntity
    {
        public string Name { get; set; }

        public string Extension { get; set; }
    }
}
