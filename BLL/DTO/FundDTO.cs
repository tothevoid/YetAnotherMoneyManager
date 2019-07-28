using MoneyManager.Common;
using System;

namespace MoneyManager.BLL.DTO
{
    public class FundDTO : BaseEntity
    {
        public string Name { get; set; }

        public double Balance { get; set; }
    }
}
