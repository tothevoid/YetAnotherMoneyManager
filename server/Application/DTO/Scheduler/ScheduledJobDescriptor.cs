using System;

namespace MoneyManager.Application.DTO.Scheduler
{
    public record ScheduledJobDescriptor(
        Type JobType,
        string TaskName,
        string DisplayName,
        string Description,
        string Category,
        string DefaultCronExpression);
}
