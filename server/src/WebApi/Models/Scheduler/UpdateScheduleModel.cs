namespace MoneyManager.WebApi.Models.Scheduler
{
    public class UpdateScheduleModel
    {
        public string CronExpression { get; set; }

        public bool? IsEnabled { get; set; }
    }
}
