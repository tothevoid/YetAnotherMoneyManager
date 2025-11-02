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
        Task<BrokerAccountSummaryDto> GetSummary(Guid brokerAccountId, DateTime from, DateTime to);

        Task<BrokerAccountDailyStatsDto> GetDailyStats(Guid brokerAccountId);

        Task<IEnumerable<BrokerAccountDayTransferDto>> GetMonthTransfersHistory(Guid brokerAccountId, int month, int year);

        Task<IEnumerable<BrokerAccountMonthTransferDto>> GetYearTransfersHistory(Guid brokerAccountId, int year);
    }
}
