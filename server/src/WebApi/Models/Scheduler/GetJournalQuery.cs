using MoneyManager.Application.Enums.Scheduler;

namespace MoneyManager.WebApi.Models.Scheduler
{
    public class GetJournalQuery
    {
        public int PageIndex { get; set; } = 1;

        public int RecordsQuantity { get; set; } = 20;

        public string TaskName { get; set; }

        public ScheduledTaskExecutionStatus? Status { get; set; }

        public ScheduledTaskTriggerSource? TriggerSource { get; set; }
    }
}
