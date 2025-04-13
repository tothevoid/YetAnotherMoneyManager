using MoneyManager.Application.DTO.Securities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface ISecurityTransactionService
    {
        Task<IEnumerable<SecurityTransactionDto>> GetAll();
        Task<Guid> Add(SecurityTransactionDto securityTransaction);
        Task Update(SecurityTransactionDto securityTransaction);
        Task Delete(Guid id);
    }
}
