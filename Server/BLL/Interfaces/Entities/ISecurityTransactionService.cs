using BLL.DTO;
using MoneyManager.BLL.DTO;
using MoneyManager.WEB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.BLL.Interfaces.Entities
{
    public interface ISecurityTransactionService
    {
        IEnumerable<SecurityTransactionDto> GetAll();
        Task<Guid> Add(SecurityTransactionDto securityTransaction);
        Task Update(SecurityTransactionDto securityTransaction);
        Task Delete(Guid id);
    }
}
