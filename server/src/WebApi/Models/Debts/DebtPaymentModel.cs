using System;
using Audex.Application.DTO.Accounts;
using Audex.Shared.Entities;
using Audex.WebApi.Models.Accounts;
using Audex.WebApi.Models.Transactions;

namespace Audex.WebApi.Models.Debts
{
    public class DebtPaymentModel: BaseEntity
    {
        public DebtModel Debt { get; set; }

        public Guid DebtId { get; set; }

        public AccountModel TargetAccount { get; set; }

        public Guid TargetAccountId { get; set; }

        public DateOnly Date { get; set; }

        public decimal Amount { get; set; }

        public bool IsPercentagePayment { get; set; }
    }
}
