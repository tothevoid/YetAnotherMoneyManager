using Audex.WebApi.Models.Common;

namespace Audex.WebApi.Models.Debts
{
    public class GetAllDebtPaymentsQuery: BasePageableQuery
    {
        public System.Guid? DebtId { get; set; }
        public System.Guid? TagId { get; set; }
    }
}
