using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audex.Infrastructure.Interfaces.Messages
{
    public interface IServerNotifier
    {
        public Task SendToAllAsync(string message = null);
    }
}
