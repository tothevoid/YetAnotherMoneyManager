namespace Audex.Application.DTO.Scheduler
{
    public class CreateScheduledTaskDto
    {
        public string TaskName { get; set; }

        public string CronExpression { get; set; }

        public bool IsEnabled { get; set; } = false;
    }
}
