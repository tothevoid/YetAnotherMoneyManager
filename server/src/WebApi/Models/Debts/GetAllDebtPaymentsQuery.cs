using MoneyManager.WebApi.Models.Common;

namespace MoneyManager.WebApi.Models.Debts
{
    public class GetAllDebtPaymentsQuery: BasePageableQuery
    {
        public System.Guid? DebtId { get; set; }
        public System.Guid? TagId { get; set; }
    }
}
