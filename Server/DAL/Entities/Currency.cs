using MoneyManager.Common;
using System;

namespace MoneyManager.DAL.Entities
{
    public class Currency : BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}