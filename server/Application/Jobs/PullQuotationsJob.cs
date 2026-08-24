using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MoneyManager.Application.Attributes.Scheduler;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.DatabaseBackup;
using MoneyManager.Application.Interfaces.Scheduler;
using TickerQ.Utilities.Base;

namespace MoneyManager.Application.Jobs
{
    [ScheduledJob(
        taskName: "PullQuotations",
        displayName: "Update MOEX Quotations",
        description: "Periodically fetch latest security quotes from MOEX exchange",
        category: "Brokers",
        defaultCronExpression: "*/15 * * * *")]
    public class PullQuotationsJob : ScheduledJobBase
    {
        private readonly IBrokerAccountSecurityService _brokerAccountSecurityService;

        public PullQuotationsJob(
            IBrokerAccountSecurityService brokerAccountSecurityService,
            IDatabaseStateService databaseStateService,
            ISchedulerJournalService journalService)
            : base(databaseStateService, journalService)
        {
            _brokerAccountSecurityService = brokerAccountSecurityService;
        }

        [TickerFunction(functionName: "PullQuotations")]
        public async Task Pull()
        {
            await ExecuteAsync(triggerSource: ScheduledTaskTriggerSource.Scheduled);
        }

        protected override async Task<JobExecutionResult> ExecuteCoreAsync(
            ScheduledTaskTriggerSource triggerSource,
            CancellationToken cancellationToken)
        {
            await _brokerAccountSecurityService.PullQuotationsAsync();
            return JobExecutionResult.Success("Successfully updated securities quotations from MOEX");
        }
    }
}

