using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountFundsTransferService
    {
        Task<IEnumerable<BrokerAccountFundsTransferDto>> GetAll(Guid brokerAccountId);

        Task<IEnumerable<BrokerAccountFundsTransferDto>> GetAll(Guid brokerAccountId, int pageIndex, int recordsQuantity);
        Task<PaginationConfigDto> GetPagination(Guid brokerAccountId);
        Task<BrokerAccountFundsTransferDto> Add(BrokerAccountFundsTransferDto transfer);
        Task Update(BrokerAccountFundsTransferDto transfer);
        Task Delete(Guid id);
    }
}
