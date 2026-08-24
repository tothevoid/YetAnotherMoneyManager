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
using MoneyManager.Application.Interfaces.Scheduler;
using MoneyManager.Infrastructure.Constants;
using MoneyManager.Infrastructure.Entities.Notifications;
using MoneyManager.Infrastructure.Entities.Scheduler;
using TickerQ.Utilities.Base;

namespace MoneyManager.Application.Jobs
{
    [ScheduledJob(
        taskName: "DatabaseBackup",
        displayName: "Database Backup",
        description: "Create PostgreSQL database dump with optional encryption",
        category: "System",
        defaultCronExpression: "0 3 * * 0")]
    public class DatabaseBackupJob : ScheduledJobBase
    {
        private readonly IDatabaseBackupService _backupService;
        private readonly IFileStorageService _fileStorageService;
        private readonly INotificationService _notificationService;
        private string _customPassword;

        public DatabaseBackupJob(
            IDatabaseBackupService backupService,
            IFileStorageService fileStorageService,
            INotificationService notificationService,
            IDatabaseStateService databaseStateService,
            ISchedulerJournalService journalService)
            : base(databaseStateService, journalService)
        {
            _backupService = backupService;
            _fileStorageService = fileStorageService;
            _notificationService = notificationService;
        }

        [TickerFunction(functionName: "DatabaseBackup")]
        public async Task BackupDatabaseAsync()
        {
            await ExecuteAsync(triggerSource: ScheduledTaskTriggerSource.Scheduled);
        }

        public async Task ExecuteBackupAsync(
            ScheduledTaskTriggerSource triggerSource = ScheduledTaskTriggerSource.Manual,
            string password = null,
            CancellationToken cancellationToken = default)
        {
            _customPassword = password;
            await ExecuteAsync(triggerSource, cancellationToken);
        }

        protected override async Task<JobExecutionResult> ExecuteCoreAsync(
            ScheduledTaskTriggerSource triggerSource,
            CancellationToken cancellationToken)
        {
            var password = _customPassword;
            _customPassword = null;

            var backup = await _backupService.CreateBackupAsync(password);
            var objectKey = $"{DateTime.UtcNow:yyyy/MM}/{backup.FileName}";

            await _fileStorageService.UploadBytesAsync(
                bucketName: "backups",
                data: backup.Data,
                key: objectKey,
                contentType: backup.ContentType);

            var attachment = new ScheduledTaskAttachment
            {
                Id = Guid.NewGuid(),
                FileName = backup.FileName,
                BucketName = "backups",
                StoragePath = objectKey,
                ContentType = backup.ContentType,
                FileSizeBytes = backup.FileSizeBytes,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationService.CreateAsync(
                title: backup.IsEncrypted ? "Encrypted DB backup ready" : "DB backup ready",
                message: $"Database backup successfully created: {backup.FileName}",
                severity: NotificationSeverity.Success,
                actionUrl: "/scheduler?tab=journal",
                category: "Scheduler",
                userProfileId: UserProfileConstants.UserProfileId);

            return JobExecutionResult.Success(
                logMessage: $"Successfully created {(backup.IsEncrypted ? "encrypted " : "")}database backup ({backup.FileSizeBytes / 1024.0:F1} KB)",
                attachment: attachment);
        }

        protected override async Task OnFailureAsync(
            ScheduledTaskTriggerSource triggerSource,
            Exception exception,
            CancellationToken cancellationToken)
        {
            await _notificationService.CreateAsync(
                title: "DB backup failed",
                message: $"Failed to create database backup: {exception.Message}",
                severity: NotificationSeverity.Danger,
                actionUrl: "/scheduler?tab=journal",
                category: "Scheduler",
                userProfileId: UserProfileConstants.UserProfileId);
        }
    }
}
