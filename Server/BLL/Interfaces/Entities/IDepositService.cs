using MoneyManager.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Entities
{
    public interface IDepositService
    {
        Task<IEnumerable<DepositDTO>> GetAll();
        
        Task<Guid> Add(DepositDTO deposit);

        Task Update(DepositDTO modifiedDeposit);

        Task Delete(DepositDTO deposit);
       
    }
}
