using MoneyManager.Application.DTO.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountService
    {
        Task<IEnumerable<BrokerAccountDTO>> GetAll();

        Task<BrokerAccountDTO> GetById(Guid id);

        Task<BrokerAccountPortfolioDto> GetPortfolioValuesByBrokerAccount(Guid brokerAccountId);

        Task<BrokerAccountPortfolioDto> GetPortfolioValues();

        Task<Guid> Add(BrokerAccountDTO brokerAccount);

        Task Update(BrokerAccountDTO brokerAccount);

        Task Delete(Guid id);
    }
}
