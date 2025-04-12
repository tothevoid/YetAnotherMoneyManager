using System;

namespace MoneyManager.WebApi.Models.Transaction
{
    public class TransactionTypeModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }
    }
}
