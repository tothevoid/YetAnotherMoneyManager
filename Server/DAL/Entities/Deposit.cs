using MoneyManager.Common;
using System;

namespace MoneyManager.DAL.Entities
{
    public class Deposit : BaseEntity
    {
        public string Name { get; set; }

        public DateOnly From { get; set; }

        public DateOnly To { get; set; }

        public decimal Percentage { get; set; }

        public decimal InitialAmount { get; set; }

        public decimal EstimatedEarn { get; set; }

        public Guid AccountId { get; set; }

        public Account Account { get; set; }

        public Deposit AssignReferences(Account account)
        {
            if (account == null)
            {
                return this;
            }

            Account = account;
            AccountId = account.Id;

            return this;
        }
    }
}