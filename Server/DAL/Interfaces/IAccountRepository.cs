using MoneyManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.DAL.Entities;

namespace MoneyManager.DAL.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        IEnumerable<Account> GetAllFull();
    }
}
