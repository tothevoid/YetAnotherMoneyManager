using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MoneyManager.Application.Attributes.Scheduler;
using MoneyManager.Application.Constants;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.DatabaseBackup;
using MoneyManager.Application.Interfaces.Localization;
using MoneyManager.Application.Interfaces.Scheduler;
using MoneyManager.Infrastructure.Constants;
using MoneyManager.Infrastructure.Interfaces.Messages;
using TickerQ.Utilities.Base;

namespace MoneyManager.Application.Jobs
{
    [ScheduledJob(
        taskName: "PullQuotations",
        displayNameKey: LocalizationKeys.Jobs.PullQuotations.Name,
        descriptionKey: LocalizationKeys.Jobs.PullQuotations.Description,
        categoryKey: LocalizationKeys.Jobs.Categories.Brokers,
        defaultCronExpression: "0 */15 * * * *")]
    public class PullQuotationsJob : ScheduledJobBase
    {
        private readonly IBrokerAccountSecurityService _brokerAccountSecurityService;
        private readonly ILocalizationService _localizer;

        public PullQuotationsJob(
            IBrokerAccountSecurityService brokerAccountSecurityService,
            ILocalizationService localizer,
            IDatabaseStateService databaseStateService,
            ISchedulerAttachmentService attachmentService,
            IServerNotifier serverNotifier)
            : base(databaseStateService, attachmentService, serverNotifier)
        {
            _brokerAccountSecurityService = brokerAccountSecurityService;
            _localizer = localizer;
        }

        [TickerFunction(functionName: "PullQuotations")]
        public async Task Pull(
            TickerFunctionContext context,
            CancellationToken cancellationToken = default)
        {
            await ExecuteAsync(
                triggerSource: ScheduledTaskTriggerSource.Scheduled,
                cancellationToken: cancellationToken,
                occurrenceId: context.Id);
        }

        protected override async Task<JobExecutionResult> ExecuteCoreAsync(
            ScheduledTaskTriggerSource triggerSource,
            CancellationToken cancellationToken)
        {
            await _brokerAccountSecurityService.PullQuotationsAsync();
            var logMessage = await _localizer.GetForUserAsync(LocalizationKeys.Scheduler.PullQuotationsSuccess, UserProfileConstants.UserProfileId);
            return JobExecutionResult.Success(logMessage);
        }
    }
}

