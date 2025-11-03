using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Application.Interfaces.Brokers;
using TickerQ.Utilities.Base;

namespace MoneyManager.Application.Jobs
{
    public class PullQuotationsJob
    {
        private readonly IBrokerAccountSecurityService _brokerAccountSecurityService;

        public PullQuotationsJob(IBrokerAccountSecurityService brokerAccountSecurityService)
        {
            _brokerAccountSecurityService = brokerAccountSecurityService;
        }

        [TickerFunction(functionName: nameof(Pull), cronExpression: "*/1 * * * *")]
        public async Task Pull()
        {
            await _brokerAccountSecurityService.PullQuotations();
        }
    }
}
