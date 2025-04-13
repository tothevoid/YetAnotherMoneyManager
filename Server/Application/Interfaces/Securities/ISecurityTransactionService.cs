using MoneyManager.Application.DTO.Securities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface ISecurityTransactionService
    {
        Task<IEnumerable<SecurityTransactionDTO>> GetAll();
        Task<Guid> Add(SecurityTransactionDTO securityTransaction);
        Task Update(SecurityTransactionDTO securityTransaction);
        Task Delete(Guid id);
    }
}
