using MoneyManager.Common;
using MoneyManager.DAL;
using System;

namespace MoneyManager.WebApi.Models.Accounts
{
    public class UpdateAccountModel
    {
        public Guid AccountId { get; set; }

        public decimal Delta { get; set; }
    }
}