namespace Audex.WebApi.Models.Scheduler
{
    public class ScheduledTaskDefinitionModel
    {
        public string TaskName { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string DefaultCronExpression { get; set; }
    }
}
