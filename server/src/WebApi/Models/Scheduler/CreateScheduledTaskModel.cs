namespace Audex.WebApi.Models.Scheduler
{
    public class CreateScheduledTaskModel
    {
        public string TaskName { get; set; }

        public string CronExpression { get; set; }

        public bool IsEnabled { get; set; } = false;
    }
}
