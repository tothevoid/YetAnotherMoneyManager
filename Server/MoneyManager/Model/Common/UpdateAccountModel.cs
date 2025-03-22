using MoneyManager.Common;
using MoneyManager.DAL;
using System;

namespace MoneyManager.Model.Common
{
    public class UpdateAccountModel
    {
        public Guid AccountId { get; set; }

        public double Delta { get; set; }
    }
}