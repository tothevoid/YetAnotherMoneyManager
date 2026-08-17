using MoneyManager.Application.DTO.Securities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Common;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface ISecurityTransactionService
    {
        Task<IEnumerable<SecurityTransactionDto>> GetAllAsync(Guid? brokerAccountId, int recordsQuantity, int pageIndex);

        Task<Dictionary<string, SecurityTransactionsSummary>> GetSummaryTillSpecificDateAsync(DateOnly date, Guid? brokerAccountId);

        Task<IEnumerable<SecurityTransactionsHistoryDto>> GetTransactionsHistoryAsync(Guid securityId);
        Task<PaginationConfigDto> GetPaginationAsync(Guid brokerAccountId);
        Task<PaginationConfigDto> GetPaginationAsync();
        Task<Guid> AddAsync(SecurityTransactionDto securityTransaction);
        Task UpdateAsync(SecurityTransactionDto securityTransaction);
        Task DeleteAsync(Guid id);
    }

    //TODO: remove from interface file
    public class SecurityTransactionsSummary
    {
        public int ActualQuantity { get; set; }

        public decimal PurchasePriceSum { get; set; }

        public decimal SellPriceSum { get; set; }
    }
}
