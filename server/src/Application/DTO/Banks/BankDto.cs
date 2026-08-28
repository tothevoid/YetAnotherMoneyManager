using System;
using Audex.Shared.Entities;

namespace Audex.Application.DTO.Banks
{
    public class BankDto: BaseEntity
    {
        public string Name { get; set; }

        public string IconKey { get; set; }
    }
}