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
        Task<IEnumerable<Transaction>> GetAllFull(int momth, int year);
    }
}