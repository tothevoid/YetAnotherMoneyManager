using System.Collections.Generic;
using TickerQ.Utilities.Entities;

namespace Audex.Infrastructure.Entities.Scheduler
{
    public class ScheduledCronTicker : CronTickerEntity
    {
        public ICollection<CronTickerOccurrenceEntity<ScheduledCronTicker>> Occurrences { get; set; } 
            = new List<CronTickerOccurrenceEntity<ScheduledCronTicker>>();
    }
}
