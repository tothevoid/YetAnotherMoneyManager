using System;
using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.DAL.Entities;

namespace MoneyManager.BLL.Mappings
{
    public class DTOToEntityProfile : Profile
    {
        public DTOToEntityProfile()
        {
            CreateMap<TransactionDTO, Transaction>().ReverseMap();
            CreateMap<FundDTO, Fund>().ReverseMap();
        }
    }
}