using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MoneyManager.Application.Attributes.Scheduler;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.DatabaseBackup;
using MoneyManager.Application.Interfaces.FileStorage;
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
        displayName: "All Assets Report (Excel)",
        description: "Automatically generate Excel statement for all accounts, assets and debts",
        category: "Reports",
        defaultCronExpression: "0 0 9 * * 1")]
    public class AssetReportJob : ScheduledJobBase
    {
        private readonly IAllAssetsReportService _reportService;
        private readonly IFileStorageService _fileStorageService;
        private readonly INotificationService _notificationService;

        public AssetReportJob(
            IAllAssetsReportService reportService,
            IFileStorageService fileStorageService,
            INotificationService notificationService,
            IDatabaseStateService databaseStateService,
            ISchedulerAttachmentService attachmentService,
            IServerNotifier serverNotifier)
            : base(databaseStateService, attachmentService, serverNotifier)
        {
            _reportService = reportService;
            _fileStorageService = fileStorageService;
            _notificationService = notificationService;
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

            await _notificationService.CreateAsync(
                title: "Assets report generated",
                message: $"Weekly assets report successfully saved ({report.FileName}). Available for download in journal.",
                severity: NotificationSeverity.Success,
                category: "Reports",
                userProfileId: UserProfileConstants.UserProfileId);

            return JobExecutionResult.Success(
                logMessage: $"Successfully generated all assets report ({report.FileName}, {report.FileSizeBytes / 1024} KB)",
                attachment: attachment);
        }

        protected override async Task OnFailureAsync(
            ScheduledTaskTriggerSource triggerSource,
            Exception exception,
            CancellationToken cancellationToken)
        {
            await _notificationService.CreateAsync(
                title: "Report generation failed",
                message: $"Failed to generate assets report: {exception.Message}",
                severity: NotificationSeverity.Danger,
                category: "Reports",
                userProfileId: UserProfileConstants.UserProfileId);
        }
    }
}
