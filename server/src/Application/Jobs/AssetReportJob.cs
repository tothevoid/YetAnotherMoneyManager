using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MoneyManager.Application.Attributes.Scheduler;
using MoneyManager.Application.Constants;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.DatabaseBackup;
using MoneyManager.Application.Interfaces.FileStorage;
using MoneyManager.Application.Interfaces.Localization;
using MoneyManager.Application.Interfaces.Notifications;
using MoneyManager.Application.Interfaces.Reports;
using MoneyManager.Application.Interfaces.Scheduler;
using MoneyManager.Infrastructure.Constants;
using MoneyManager.Infrastructure.Entities.Notifications;
using MoneyManager.Infrastructure.Entities.Scheduler;
using MoneyManager.Infrastructure.Interfaces.Messages;
using TickerQ.Utilities.Base;

namespace MoneyManager.Application.Jobs
{
    [ScheduledJob(
        taskName: "GenerateAllAssetsReport",
        displayNameKey: LocalizationKeys.Jobs.AssetReport.Name,
        descriptionKey: LocalizationKeys.Jobs.AssetReport.Description,
        categoryKey: LocalizationKeys.Jobs.Categories.Reports,
        defaultCronExpression: "0 0 9 * * 1")]
    public class AssetReportJob : ScheduledJobBase
    {
        private readonly IAllAssetsReportService _reportService;
        private readonly IFileStorageService _fileStorageService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizer;

        public AssetReportJob(
            IAllAssetsReportService reportService,
            IFileStorageService fileStorageService,
            INotificationService notificationService,
            ILocalizationService localizer,
            IDatabaseStateService databaseStateService,
            ISchedulerAttachmentService attachmentService,
            IServerNotifier serverNotifier)
            : base(databaseStateService, attachmentService, serverNotifier)
        {
            _reportService = reportService;
            _fileStorageService = fileStorageService;
            _notificationService = notificationService;
            _localizer = localizer;
        }

        [TickerFunction(functionName: "GenerateAllAssetsReport")]
        public async Task GenerateReportAsync(
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
            var report = await _reportService.CreateReportAsync();
            var objectKey = $"{DateTime.UtcNow:yyyy/MM}/{report.FileName}";

            await _fileStorageService.UploadBytesAsync(
                bucketName: "reports",
                data: report.Data,
                key: objectKey,
                contentType: report.ContentType);

            var attachment = new ScheduledTaskAttachment
            {
                Id = Guid.NewGuid(),
                FileName = report.FileName,
                BucketName = "reports",
                StoragePath = objectKey,
                ContentType = report.ContentType,
                FileSizeBytes = report.FileSizeBytes,
                CreatedAt = DateTime.UtcNow
            };

            var title = await _localizer.GetForUserAsync(LocalizationKeys.Notifications.ReportGeneratedTitle, UserProfileConstants.UserProfileId);
            var message = await _localizer.GetForUserAsync(LocalizationKeys.Notifications.ReportGeneratedMessage, UserProfileConstants.UserProfileId, report.FileName);

            await _notificationService.CreateAsync(
                title: title,
                message: message,
                severity: NotificationSeverity.Success,
                category: "Reports",
                userProfileId: UserProfileConstants.UserProfileId);

            var logMessage = await _localizer.GetForUserAsync(LocalizationKeys.Scheduler.AssetReportSuccess, UserProfileConstants.UserProfileId, report.FileName, report.FileSizeBytes / 1024);

            return JobExecutionResult.Success(
                logMessage: logMessage,
                attachment: attachment);
        }

        protected override async Task OnFailureAsync(
            ScheduledTaskTriggerSource triggerSource,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var title = await _localizer.GetForUserAsync(LocalizationKeys.Notifications.ReportGenerationFailedTitle, UserProfileConstants.UserProfileId);
            var message = await _localizer.GetForUserAsync(LocalizationKeys.Notifications.ReportGenerationFailedMessage, UserProfileConstants.UserProfileId, exception.Message);

            await _notificationService.CreateAsync(
                title: title,
                message: message,
                severity: NotificationSeverity.Danger,
                category: "Reports",
                userProfileId: UserProfileConstants.UserProfileId);
        }
    }
}
