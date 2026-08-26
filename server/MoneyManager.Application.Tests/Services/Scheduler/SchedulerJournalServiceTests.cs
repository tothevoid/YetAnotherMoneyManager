using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.Scheduler;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Entities.Scheduler;
using TickerQ.Utilities.Entities;
using Xunit;

namespace MoneyManager.Application.Tests.Services.Scheduler
{
    public class SchedulerJournalServiceTests : TestBase
    {
        public SchedulerJournalServiceTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
        }

        [Fact]
        public async Task GetJournalAsync_And_GetJournalPaginationAsync_WorksCorrectly()
        {
            await ExecuteScopeAsync(async sp =>
            {
                var taskService = sp.GetRequiredService<ISchedulerTaskService>();
                var journalService = sp.GetRequiredService<ISchedulerJournalService>();
                var db = sp.GetRequiredService<MoneyManager.Infrastructure.Interfaces.Database.IUnitOfWork>();
                var occurrenceRepo = db.CreateRepository<CronTickerOccurrenceEntity<ScheduledCronTicker>>();
                var tickerRepo = db.CreateRepository<ScheduledCronTicker>();
                var attachmentService = sp.GetRequiredService<ISchedulerAttachmentService>();

                await taskService.CreateTaskAsync(new CreateScheduledTaskDto
                {
                    TaskName = "GenerateAllAssetsReport",
                    CronExpression = "0 0 * * *",
                    IsEnabled = true
                });

                var ticker = await tickerRepo.FindAsync(t => t.Function == "GenerateAllAssetsReport");
                Assert.NotNull(ticker);

                var occurrenceId = Guid.NewGuid();
                var occurrence = new CronTickerOccurrenceEntity<ScheduledCronTicker>
                {
                    Id = occurrenceId,
                    CronTickerId = ticker.Id,
                    ExecutionTime = DateTime.UtcNow,
                    ExecutedAt = DateTime.UtcNow,
                    ElapsedTime = 150,
                    Status = TickerQ.Utilities.Enums.TickerStatus.Done,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await occurrenceRepo.AddAsync(occurrence);
                await db.CommitAsync();

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

                await attachmentService.SaveAttachmentAsync(occurrenceId, attachment);

                var journal = await journalService.GetJournalAsync(1, 10, "GenerateAllAssetsReport", status: ScheduledTaskExecutionStatus.Done);
                Assert.NotNull(journal);
                var list = journal.ToList();
                Assert.NotEmpty(list);

                var entry = list.FirstOrDefault(j => j.Id == occurrenceId);
                Assert.NotNull(entry);
                Assert.Equal(ScheduledTaskExecutionStatus.Done, entry.Status);
                Assert.NotEmpty(entry.Attachments);

                var pagination = await journalService.GetJournalPaginationAsync("GenerateAllAssetsReport", status: ScheduledTaskExecutionStatus.Done);
                Assert.NotNull(pagination);
                Assert.True(pagination.RecordsQuantity > 0);
            });
        }
    }
}
