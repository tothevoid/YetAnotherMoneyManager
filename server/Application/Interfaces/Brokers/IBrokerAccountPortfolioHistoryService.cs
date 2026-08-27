using System;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Brokers;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountPortfolioHistoryService
    {
        Task<BrokerAccountPortfolioHistoryDto> GetAllAsync(DateOnly date);
        Task<BrokerAccountPortfolioHistoryDto> GetByBrokerAccountAsync(DateOnly date, Guid brokerAccountId);
    }
}
