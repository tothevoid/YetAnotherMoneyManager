using System;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO.Banks
{
    public class BankDto: BaseEntity
    {
        public string Name { get; set; }
    }
}