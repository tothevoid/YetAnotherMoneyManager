using MoneyManager.Common;
using System;

namespace MoneyManager.BLL.DTO
{
    public class UpdateFundDTO
    {
        public Guid FundId { get; set; }

        public double Delta { get; set; }
    }
}
