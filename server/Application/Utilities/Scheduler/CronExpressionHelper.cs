using System;
using System.Linq;
using Cronos;

namespace MoneyManager.Application.Utilities.Scheduler
{
    public static class CronExpressionHelper
    {
        public static bool IsValidCronExpression(string cronExpression)
        {
            if (string.IsNullOrWhiteSpace(cronExpression))
            {
                return false;
            }

            try
            {
                CronExpression.Parse(cronExpression.Trim(), CronFormat.Standard);
                return true;
            }
            catch (CronFormatException)
            {
                return false;
            }
        }

        public static DateTime? GetNextExecutionUtc(string cronExpression, DateTime fromUtc)
        {
            if (string.IsNullOrWhiteSpace(cronExpression))
            {
                return null;
            }

            try
            {
                var expression = CronExpression.Parse(cronExpression.Trim(), CronFormat.Standard);
                return expression.GetNextOccurrence(fromUtc, TimeZoneInfo.Utc);
            }
            catch (CronFormatException)
            {
                return null;
            }
        }

        public static DateTime? GetLastExecutionUtc(string cronExpression, DateTime toUtc)
        {
            if (string.IsNullOrWhiteSpace(cronExpression))
            {
                return null;
            }

            try
            {
                var expression = CronExpression.Parse(cronExpression.Trim(), CronFormat.Standard);
                var fromUtc = toUtc.AddDays(-30);
                var occurrences = expression.GetOccurrences(fromUtc, toUtc, TimeZoneInfo.Utc).ToList();
                return occurrences.Count > 0 ? occurrences[^1] : null;
            }
            catch (CronFormatException)
            {
                return null;
            }
        }
    }
}
