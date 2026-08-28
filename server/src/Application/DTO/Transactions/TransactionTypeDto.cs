using System;
using Audex.Shared.Entities;

namespace Audex.Application.DTO.Transactions
{
    public class TransactionTypeDto: BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        public string IconKey { get; set; }
    }
}
