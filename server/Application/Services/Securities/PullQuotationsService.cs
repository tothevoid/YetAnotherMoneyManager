using System;
using MoneyManager.Application.Interfaces.Securities;

namespace MoneyManager.Application.Services.Securities
{
    public class PullQuotationsService: IPullQuotationsService
    {
        private DateTime? _lastPullDate = null;
        private readonly object _lock = new();

        public DateTime? LastPullDate
        {
            get
            {
                lock (_lock)
                {
                    return _lastPullDate;
                }
            }
        }

        public void UpdatePullDate(DateTime newDate)
        {
            lock (_lock)
            {
                _lastPullDate = newDate;
            }
        }
    }
}
