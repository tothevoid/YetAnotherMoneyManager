using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.WEB.Model
{
    public class TransactionTypeModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }
    }
}
