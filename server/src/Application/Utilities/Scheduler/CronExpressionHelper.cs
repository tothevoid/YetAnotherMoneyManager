using System;
using System.Linq;
using Cronos;
using CronosExpression = Cronos.CronExpression;
using CronExpression = TickerQ.Utilities.CronExpression;

namespace Audex.Application.Utilities.Scheduler
{
    public static class CronExpressionHelper
    {
        public static string ToTickerQCron(string cronExpression)
        {
            if (string.IsNullOrWhiteSpace(cronExpression))
            {
                return cronExpression;
            }

            if (CronExpression.TryParse(cronExpression, out var parsed))
            {
                return parsed; // Expands 5-part to 6-part ("0 ...")
            }

            return cronExpression.Trim();
        }

        public static string ToStandardCron(string cronExpression)
        {
            if (string.IsNullOrWhiteSpace(cronExpression))
            {
                return cronExpression;
            }

            var parts = cronExpression.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 6 && parts[0] == "0")
            {
                return string.Join(" ", parts.Skip(1));
            }

            return cronExpression.Trim();
        }

        public static bool IsValidCronExpression(string cronExpression)
        {
            if (string.IsNullOrWhiteSpace(cronExpression))
            {
                return false;
            }

            if (!CronExpression.TryParse(cronExpression, out var tickerParsed))
            {
                return false;
            }

            try
            {
                var parsedStr = tickerParsed.ToString();
                var format = parsedStr.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length == 6
                    ? CronFormat.IncludeSeconds
                    : CronFormat.Standard;
                CronosExpression.Parse(parsedStr, format);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static DateTime? GetNextExecutionUtc(string cronExpression, DateTime fromUtc)
        {
            var tickerQCron = ToTickerQCron(cronExpression);
            if (!CronExpression.TryParse(tickerQCron, out var parsed))
            {
                return null;
            }

            try
            {
                var expression = CronosExpression.Parse(parsed, CronFormat.IncludeSeconds);
                return expression.GetNextOccurrence(fromUtc, TimeZoneInfo.Utc);
            }
            catch (CronFormatException)
            {
                return null;
            }
        }

        public static DateTime? GetLastExecutionUtc(string cronExpression, DateTime toUtc)
        {
            var tickerQCron = ToTickerQCron(cronExpression);
            if (!CronExpression.TryParse(tickerQCron, out var parsed))
            {
                return null;
            }

            try
            {
                var expression = CronosExpression.Parse(parsed, CronFormat.IncludeSeconds);
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
