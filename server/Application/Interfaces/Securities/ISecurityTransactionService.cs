using MoneyManager.Application.DTO.Securities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Common;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface ISecurityTransactionService
    {
        Task<IEnumerable<SecurityTransactionDTO>> GetAll(Guid? brokerAccountId, int recordsQuantity, int pageIndex);
        Task<IEnumerable<SecurityTransactionsHistoryDto>> GetTransactionsHistory(Guid securityId);
        Task<PaginationConfigDto> GetPagination(Guid brokerAccountId);
        Task<PaginationConfigDto> GetPagination();
        Task<Guid> Add(SecurityTransactionDTO securityTransaction);
        Task Update(SecurityTransactionDTO securityTransaction);
        Task Delete(Guid id);

    }
}
