using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.Scheduler;
using MoneyManager.Application.Utilities.Scheduler;
using MoneyManager.Infrastructure.Entities.Scheduler;
using MoneyManager.Infrastructure.Interfaces.Database;
using TickerQ.Utilities.Entities;
using TickerQ.Utilities.Interfaces.Managers;
using TickerQ.Utilities.Enums;

namespace MoneyManager.Application.Services.Scheduler
{
    public class SchedulerTaskService : ISchedulerTaskService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<ScheduledCronTicker> _tickerRepo;
        private readonly IRepository<CronTickerOccurrenceEntity<ScheduledCronTicker>> _occurrenceRepo;
        private readonly IScheduledJobRegistry _jobRegistry;
        private readonly ILogger<SchedulerTaskService> _logger;

        public SchedulerTaskService(
            IUnitOfWork unitOfWork,
            IScheduledJobRegistry jobRegistry,
            ILogger<SchedulerTaskService> logger)
        {
            _db = unitOfWork;
            _tickerRepo = unitOfWork.CreateRepository<ScheduledCronTicker>();
            _occurrenceRepo = unitOfWork.CreateRepository<CronTickerOccurrenceEntity<ScheduledCronTicker>>();
            _jobRegistry = jobRegistry;
            _logger = logger;
        }

        public async Task<IEnumerable<ScheduledTaskDefinitionDto>> GetNotScheduledTasksAsync()
        {
            var tickers = await _tickerRepo.GetAllAsync(disableTracking: true);
            var configuredNames = tickers
                .Select(ticker => ticker.Function)
                .Where(functionName => !string.IsNullOrEmpty(functionName));
            var configuredSet = new HashSet<string>(configuredNames, StringComparer.OrdinalIgnoreCase);

            var descriptors = _jobRegistry.GetAllDescriptors();
            return descriptors
                .Where(descriptor => !configuredSet.Contains(descriptor.TaskName))
                .Select(descriptor => new ScheduledTaskDefinitionDto
                {
                    TaskName = descriptor.TaskName,
                    DisplayName = descriptor.DisplayName,
                    Description = descriptor.Description,
                    Category = descriptor.Category,
                    DefaultCronExpression = descriptor.DefaultCronExpression
                }).ToList();
        }

        public async Task<IEnumerable<ScheduledTaskDto>> GetAllTasksAsync()
        {
            var tickers = await _tickerRepo.GetAllAsync(
                include: GetFullHierarchyColumns,
                disableTracking: true);
            var currentUtcTime = DateTime.UtcNow;

            return tickers
                .Where(ticker => _jobRegistry.TryGetDescriptor(ticker.Function, out _))
                .Select(ticker =>
                {
                    var descriptor = _jobRegistry.TryGetDescriptor(ticker.Function, out var jobDescriptor) ? jobDescriptor : null;
                    var latestOccurrence = ticker.Occurrences.FirstOrDefault();
                    return MapToDto(ticker, descriptor, latestOccurrence, currentUtcTime);
                })
                .ToList();
        }

        public async Task<ScheduledTaskDto> GetTaskByNameAsync(string taskName)
        {
            if (string.IsNullOrWhiteSpace(taskName))
            {
                return null;
            }

            var tickers = await _tickerRepo.GetAllAsync(
                filter: ticker => ticker.Function == taskName,
                include: GetFullHierarchyColumns,
                disableTracking: true);

            var ticker = tickers.FirstOrDefault();
            if (ticker == null)
            {
                return null;
            }

            var descriptor = _jobRegistry.TryGetDescriptor(taskName, out var jobDescriptor) ? jobDescriptor : null;
            var latestOccurrence = ticker.Occurrences.FirstOrDefault();
            return MapToDto(ticker, descriptor, latestOccurrence, DateTime.UtcNow);
        }

        public async Task<ScheduledTaskDto> CreateTaskAsync(CreateScheduledTaskDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (!_jobRegistry.TryGetDescriptor(dto.TaskName, out var descriptor))
            {
                throw new ArgumentException($"Unknown task type: '{dto.TaskName}'", nameof(dto.TaskName));
            }

            var cronExpression = string.IsNullOrWhiteSpace(dto.CronExpression)
                ? descriptor.DefaultCronExpression
                : dto.CronExpression;

            if (!CronExpressionHelper.IsValidCronExpression(cronExpression))
            {
                throw new ArgumentException($"Invalid Cron expression: '{cronExpression}'", nameof(dto.CronExpression));
            }

            var tickerCronExpression = CronExpressionHelper.ToTickerQCron(cronExpression);

            var existingTicker = await _tickerRepo.FindAsync(ticker => ticker.Function == dto.TaskName);

            if (existingTicker != null)
            {
                throw new InvalidOperationException($"Task '{dto.TaskName}' is already created. Only one schedule per task is allowed.");
            }

            var newTicker = new ScheduledCronTicker
            {
                Id = Guid.NewGuid(),
                Description = descriptor.DisplayName,
                Function = descriptor.TaskName,
                Expression = tickerCronExpression,
                IsEnabled = dto.IsEnabled
            };

            await _tickerRepo.AddAsync(newTicker);
            await _db.CommitAsync();

            return MapToDto(newTicker, descriptor, null, DateTime.UtcNow);
        }

        public async Task<ScheduledTaskDto> UpdateScheduleAsync(string taskName, UpdateScheduleDto dto)
        {
            if (string.IsNullOrWhiteSpace(taskName) || dto == null)
            {
                return null;
            }

            var ticker = await _tickerRepo.FindAsync(ticker => ticker.Function == taskName);

            if (ticker == null)
            {
                return null;
            }

            bool deletePendingOccurrences = false;

            if (!string.IsNullOrWhiteSpace(dto.CronExpression))
            {
                if (!CronExpressionHelper.IsValidCronExpression(dto.CronExpression))
                {
                    throw new ArgumentException($"Invalid Cron expression: '{dto.CronExpression}'", nameof(dto.CronExpression));
                }

                var newExpression = CronExpressionHelper.ToTickerQCron(dto.CronExpression);
                if (ticker.Expression != newExpression)
                {
                    ticker.Expression = newExpression;
                    deletePendingOccurrences = true;
                }
            }

            if (dto.IsEnabled.HasValue)
            {
                ticker.IsEnabled = dto.IsEnabled.Value;
                if (!ticker.IsEnabled)
                {
                    deletePendingOccurrences = true;
                }
            }

            if (deletePendingOccurrences) 
            {
                await RemovePendingOccurrencesAsync(ticker.Id);
            }

            _tickerRepo.Update(ticker);
            await _db.CommitAsync();

            var descriptor = _jobRegistry.TryGetDescriptor(taskName, out var jobDescriptor) ? jobDescriptor : null;
            return MapToDto(ticker, descriptor, null, DateTime.UtcNow);
        }

        public async Task<ScheduledTaskDto> ToggleTaskStatusAsync(string taskName, bool isEnabled)
        {
            if (string.IsNullOrWhiteSpace(taskName))
            {
                return null;
            }

            var ticker = await _tickerRepo.FindAsync(ticker => ticker.Function == taskName);

            if (ticker == null)
            {
                return null;
            }

            ticker.IsEnabled = isEnabled;
            if (!isEnabled)
            {
                await RemovePendingOccurrencesAsync(ticker.Id);
            }

            _tickerRepo.Update(ticker);
            await _db.CommitAsync();

            var descriptor = _jobRegistry.TryGetDescriptor(taskName, out var jobDescriptor) ? jobDescriptor : null;
            return MapToDto(ticker, descriptor, null, DateTime.UtcNow);
        }

        public async Task<bool> DeleteTaskAsync(string taskName)
        {
            if (string.IsNullOrWhiteSpace(taskName))
            {
                return false;
            }

            var ticker = await _tickerRepo.FindAsync(ticker => ticker.Function == taskName);

            if (ticker == null)
            {
                return false;
            }

            await RemovePendingOccurrencesAsync(ticker.Id);
            await _tickerRepo.DeleteAsync(ticker.Id);
            await _db.CommitAsync();
            return true;
        }

        private async Task RemovePendingOccurrencesAsync(Guid tickerId)
        {
            var pendingOccurrences = await _occurrenceRepo.GetAllAsync(
                occurrence => occurrence.CronTickerId == tickerId &&
                              (occurrence.Status == TickerStatus.Idle || occurrence.Status == TickerStatus.Queued));

            foreach (var pending in pendingOccurrences)
            {
                await _occurrenceRepo.DeleteAsync(pending.Id);
            }
        }

        private static ScheduledTaskDto MapToDto(
            ScheduledCronTicker ticker,
            ScheduledJobDescriptor descriptor,
            CronTickerOccurrenceEntity<ScheduledCronTicker> latestOccurrence,
            DateTime currentUtcTime)
        {
            var taskName = descriptor?.TaskName ?? ticker.Function;
            var displayName = descriptor?.DisplayName ?? taskName;
            var description = descriptor?.Description ?? ticker.Description ?? string.Empty;
            var category = descriptor?.Category ?? "General";
            var clientCronExpression = CronExpressionHelper.ToStandardCron(ticker.Expression);

            var lastStatus = latestOccurrence != null
                ? SchedulerStatusMapper.ToExecutionStatus(latestOccurrence.Status)
                : ScheduledTaskExecutionStatus.Unknown;

            return new ScheduledTaskDto
            {
                TaskName = taskName,
                DisplayName = displayName,
                Description = description,
                CronExpression = clientCronExpression,
                IsEnabled = ticker.IsEnabled,
                Category = category,
                LastExecutionUtc = latestOccurrence?.ExecutedAt,
                LastExecutionStatus = lastStatus,
                LastExecutionDurationMs = latestOccurrence?.ElapsedTime,
                NextExecutionUtc = ticker.IsEnabled ? CronExpressionHelper.GetNextExecutionUtc(ticker.Expression, currentUtcTime) : null
            };
        }

        private static IQueryable<ScheduledCronTicker> GetFullHierarchyColumns(IQueryable<ScheduledCronTicker> query)
        {
            return query.Include(ticker => ticker.Occurrences
                .OrderByDescending(occurrence => occurrence.ExecutedAt ?? occurrence.ExecutionTime)
                .Take(1));
        }
    }
}
