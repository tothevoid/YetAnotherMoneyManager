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
        Task<BrokerAccountSummaryDto> GetSummary();

        Task<BrokerAccountSummaryDto> GetSummaryByBrokerAccount(Guid brokerAccountId);

        Task<BrokerAccountDailyStatsDto> GetDailyStatsByBrokerAccount(Guid brokerAccountId);

        Task<BrokerAccountDailyStatsDto> GetDailyStats();

        Task<BrokerAccountPortfolioDto> GetPortfolioValuesByBrokerAccount(Guid brokerAccountId);

        Task<BrokerAccountPortfolioDto> GetPortfolioValues();

        Task<IEnumerable<BrokerAccountDayTransferDto>> GetMonthTransfersHistory(int month, int year);

        Task<IEnumerable<BrokerAccountDayTransferDto>> GetMonthTransfersHistoryByBrokerAccount(Guid brokerAccountId, int month, int year);

        Task<IEnumerable<BrokerAccountMonthTransferDto>> GetYearTransfersHistory(int year);

        Task<IEnumerable<BrokerAccountMonthTransferDto>> GetYearTransfersHistoryByBrokerAccount(Guid brokerAccountId, int year);
    }
}
