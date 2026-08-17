using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountFundsTransferService
    {
        Task<IEnumerable<BrokerAccountFundsTransferDto>> GetAllAsync();
        Task<IEnumerable<BrokerAccountFundsTransferDto>> GetAllAsync(Guid brokerAccountId);
        Task<IEnumerable<BrokerAccountFundsTransferDto>> GetAllAsync(Guid? brokerAccountId, int pageIndex, int recordsQuantity);

        Task<(decimal deposited, decimal withdrawn)> GetSumTillSpecificDateAsync(DateOnly date, Guid? brokerAccountId);

        Task<PaginationConfigDto> GetPaginationAsync();
        Task<PaginationConfigDto> GetPaginationByBrokerAccountAsync(Guid brokerAccountId);
        Task<BrokerAccountFundsTransferDto> AddAsync(BrokerAccountFundsTransferDto transfer);
        Task UpdateAsync(BrokerAccountFundsTransferDto transfer);
        Task DeleteAsync(Guid id);
    }
}
