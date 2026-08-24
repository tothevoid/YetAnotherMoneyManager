using MoneyManager.Infrastructure.Entities.Scheduler;

namespace MoneyManager.Application.DTO.Scheduler
{
    public class JobExecutionResult
    {
        public string LogMessage { get; set; }
        public ScheduledTaskAttachment Attachment { get; set; }

        public JobExecutionResult(string logMessage = null, ScheduledTaskAttachment attachment = null)
        {
            LogMessage = logMessage;
            Attachment = attachment;
        }

        public static JobExecutionResult Success(string logMessage = null, ScheduledTaskAttachment attachment = null) =>
            new(logMessage, attachment);
    }
}
