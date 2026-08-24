using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.Scheduler;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Entities.Scheduler;
using Xunit;

namespace MoneyManager.Application.Tests.Services.Scheduler
{
    public class SchedulerJournalServiceTests : TestBase
    {
        public SchedulerJournalServiceTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
        }

        [Fact]
        public async Task RecordExecutionAsync_And_GetJournalAsync_WorksCorrectly()
        {
            await ExecuteScopeAsync(async sp =>
            {
                var taskService = sp.GetRequiredService<ISchedulerTaskService>();
                var journalService = sp.GetRequiredService<ISchedulerJournalService>();

                await taskService.CreateTaskAsync(new CreateScheduledTaskDto
                {
                    TaskName = "GenerateAllAssetsReport",
                    CronExpression = "0 0 * * *",
                    IsEnabled = true
                });

                var occurrenceId = Guid.NewGuid();

                var attachment = new ScheduledTaskAttachment
                {
                    Id = Guid.NewGuid(),
                    OccurrenceId = occurrenceId,
                    FileName = "TestReport.xlsx",
                    BucketName = "reports",
                    StoragePath = "TestReport.xlsx",
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    FileSizeBytes = 1024,
                    CreatedAt = DateTime.UtcNow
                };

                await journalService.RecordExecutionAsync(
                    taskName: "GenerateAllAssetsReport",
                    status: ScheduledTaskExecutionStatus.Success,
                    durationMs: 150,
                    triggerSource: ScheduledTaskTriggerSource.Manual,
                    logMessage: "Test run successful",
                    errorMessage: null,
                    attachment: attachment);

                var journal = await journalService.GetJournalAsync(1, 10, "GenerateAllAssetsReport", status: ScheduledTaskExecutionStatus.Success, triggerSource: ScheduledTaskTriggerSource.Manual);
                Assert.NotNull(journal);
                var list = journal.ToList();
                Assert.NotEmpty(list);

                var entry = list.FirstOrDefault(j => j.Id == occurrenceId);
                Assert.NotNull(entry);
                Assert.Equal(ScheduledTaskExecutionStatus.Success, entry.Status);
                Assert.Equal(ScheduledTaskTriggerSource.Manual, entry.TriggerSource);
                Assert.NotEmpty(entry.Attachments);

                var pagination = await journalService.GetJournalPaginationAsync("GenerateAllAssetsReport", status: ScheduledTaskExecutionStatus.Success, triggerSource: ScheduledTaskTriggerSource.Manual);
                Assert.NotNull(pagination);
                Assert.True(pagination.RecordsQuantity > 0);
            });
        }

        [Fact]
        public async Task RecordExecutionAsync_UnregisteredTask_ReturnsNull()
        {
            await ExecuteScopeAsync(async sp =>
            {
                var journalService = sp.GetRequiredService<ISchedulerJournalService>();

                var result = await journalService.RecordExecutionAsync(
                    taskName: "UnregisteredTaskName",
                    status: ScheduledTaskExecutionStatus.Success,
                    durationMs: 100);

                Assert.Null(result);
            });
        }
    }
}
