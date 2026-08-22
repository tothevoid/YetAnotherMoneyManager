using System.Threading.Tasks;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.DatabaseBackup;
using TickerQ.Utilities.Base;

namespace MoneyManager.Application.Jobs
{
    public class PullQuotationsJob
    {
        private readonly IBrokerAccountSecurityService _brokerAccountSecurityService;
        private readonly IDatabaseStateService _databaseStateService;

        public PullQuotationsJob(
            IBrokerAccountSecurityService brokerAccountSecurityService,
            IDatabaseStateService databaseStateService)
        {
            _brokerAccountSecurityService = brokerAccountSecurityService;
            _databaseStateService = databaseStateService;
        }

        [TickerFunction(functionName: nameof(Pull), cronExpression: "*/1 * * * *")]
        public async Task Pull()
        {
            if (_databaseStateService.IsRestoring)
            {
                return;
            }

            await _brokerAccountSecurityService.PullQuotationsAsync();
        }
    }
}
