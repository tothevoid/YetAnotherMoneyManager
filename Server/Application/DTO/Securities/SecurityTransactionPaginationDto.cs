using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.DTO.Securities
{
    public class SecurityTransactionPaginationDto
    {
        public int PageSize { get; set; }

        public int PagesQuantity { get; set; }
    }
}
