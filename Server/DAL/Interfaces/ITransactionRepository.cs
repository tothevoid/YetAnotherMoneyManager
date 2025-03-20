using MoneyManager.DAL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoneyManager.DAL.Interfaces
{
    public interface ITransactionRepository: IRepository<Transaction>
    {
        IEnumerable<Transaction> GetAllFull(int momth, int year);

        Task<IEnumerable<string>> GetTypes();
    }
}