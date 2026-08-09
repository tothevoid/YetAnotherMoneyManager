using System;
using System.Threading.Tasks;
using MoneyManager.Application.Services.Brokers;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountPortfolioHistoryService
    {
        Task<BrokerAccountPortfolioHistoryDto> GetAll(DateOnly date);
        Task<BrokerAccountPortfolioHistoryDto> GetByBrokerAccount(DateOnly date, Guid brokerAccountId);
    }
}
