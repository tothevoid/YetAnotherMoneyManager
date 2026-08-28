using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Services.Brokers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountSummaryService
    {
        Task<BrokerAccountSummaryDto> GetSummaryAsync();

        Task<BrokerAccountSummaryDto> GetSummaryByBrokerAccountAsync(Guid brokerAccountId);

        Task<BrokerAccountDailyStatsDto> GetDailyStatsByBrokerAccountAsync(Guid brokerAccountId);

        Task<BrokerAccountDailyStatsDto> GetDailyStatsAsync();

        Task<BrokerAccountPortfolioDto> GetPortfolioValuesByBrokerAccountAsync(Guid brokerAccountId);

        Task<BrokerAccountPortfolioDto> GetPortfolioValuesAsync();

        Task<IEnumerable<BrokerAccountDayTransferDto>> GetMonthTransfersHistoryAsync(int month, int year);

        Task<IEnumerable<BrokerAccountDayTransferDto>> GetMonthTransfersHistoryByBrokerAccountAsync(Guid brokerAccountId, int month, int year);

        Task<IEnumerable<BrokerAccountMonthTransferDto>> GetYearTransfersHistoryAsync(int year);

        Task<IEnumerable<BrokerAccountMonthTransferDto>> GetYearTransfersHistoryByBrokerAccountAsync(Guid brokerAccountId, int year);
    }
}
