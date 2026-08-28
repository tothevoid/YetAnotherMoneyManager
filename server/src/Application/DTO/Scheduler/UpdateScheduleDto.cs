namespace Audex.Application.DTO.Scheduler
{
    public class UpdateScheduleDto
    {
        public string CronExpression { get; set; }

        public bool? IsEnabled { get; set; }
    }
}
