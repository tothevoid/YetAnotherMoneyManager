using System;

namespace MoneyManager.Application.DTO.Scheduler
{
    public record ScheduledJobDescriptor(
        Type JobType,
        string TaskName,
        string DisplayNameKey,
        string DescriptionKey,
        string Category,
        string DefaultCronExpression);
}
