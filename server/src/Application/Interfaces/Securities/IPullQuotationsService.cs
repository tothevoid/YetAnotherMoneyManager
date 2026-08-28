using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface IPullQuotationsService
    {
        DateTime? LastPullDate { get; }

        void UpdatePullDate(DateTime newDate);
    }
}
