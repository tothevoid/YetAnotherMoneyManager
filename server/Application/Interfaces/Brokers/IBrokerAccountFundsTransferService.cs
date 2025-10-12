using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Infrastructure.Entities.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountFundsTransferService
    {
        Task<IEnumerable<BrokerAccountFundsTransferDto>> GetAllAsync(Guid brokerAccountId, bool income);
        Task<BrokerAccountFundsTransferDto> Add(BrokerAccountFundsTransferDto transfer);
        Task Update(BrokerAccountFundsTransferDto transfer);
        Task Delete(Guid id);
    }
}
