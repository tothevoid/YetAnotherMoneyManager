using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.WEB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.WEB.Mappings
{
    public class ViewToDTOProfile : Profile
    {
        public ViewToDTOProfile()
        {
            CreateMap<TransactionModel, TransactionDTO>().ReverseMap();
            CreateMap<FundModel, FundDTO>().ReverseMap();
        }
    }
}
