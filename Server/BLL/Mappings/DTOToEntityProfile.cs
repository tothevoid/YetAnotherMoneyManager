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
            CreateMap<AccountDTO, Account>().ReverseMap();
            CreateMap<TransactionTypeDTO, TransactionType>().ReverseMap();
            CreateMap<DepositDTO, Deposit>().ReverseMap();
        }
    }
}