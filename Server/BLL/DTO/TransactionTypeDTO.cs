using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyManager.BLL.DTO
{
    public class TransactionTypeDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }
    }
}
