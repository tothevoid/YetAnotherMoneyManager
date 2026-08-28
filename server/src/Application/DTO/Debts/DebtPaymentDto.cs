using System;
using Audex.Application.DTO.Accounts;
using Audex.Application.DTO.Transactions;
using Audex.Shared.Entities;

namespace Audex.Application.DTO.Debts
{
    public class DebtPaymentDto: BaseEntity
    {
        public DebtDto Debt { get; set; }

        public Guid DebtId { get; set; }

        public AccountDto TargetAccount { get; set; }

        public Guid TargetAccountId { get; set; }

        public DateOnly Date { get; set; }

        public decimal Amount { get; set; }

        public bool IsPercentagePayment { get; set; }
    }
}
