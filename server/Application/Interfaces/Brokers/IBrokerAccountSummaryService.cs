using MoneyManager.Application.DTO.Brokers;
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
    }
}
