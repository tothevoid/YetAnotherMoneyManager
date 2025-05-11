using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Infrastructure.Interfaces.Messages
{
    public interface IServerNotifier
    {
        public Task SendToAll(string message = null);
    }
}
