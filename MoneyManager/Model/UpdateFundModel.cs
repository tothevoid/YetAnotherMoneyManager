using MoneyManager.Common;
using MoneyManager.DAL;
using System;

namespace MoneyManager.WEB.Model
{
    public class UpdateFundModel
    {
        public Guid FundId { get; set; }

        public double Delta { get; set; }
    }
}