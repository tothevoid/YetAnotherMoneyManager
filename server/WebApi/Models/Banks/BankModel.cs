using System;
using MoneyManager.Shared.Entities;

namespace MoneyManager.WebApi.Models.Banks
{
    public class BankModel: BaseEntity
    {
        public string Name { get; set; }
    }
}