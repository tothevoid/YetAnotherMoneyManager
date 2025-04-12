using System;
using AutoMapper;
using BLL.DTO;
using MoneyManager.BLL.DTO;
using MoneyManager.DAL.Entities;
using MoneyManager.Model.Deposits;

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
            CreateMap<CurrencyDTO, Currency>().ReverseMap();
            CreateMap<AccountTypeDTO, AccountType>().ReverseMap();

            CreateMap<Deposit, ServerDepositDto>().ReverseMap();
            CreateMap<ClientDepositDto, Deposit>().ReverseMap();

            CreateMap<SecurityDto, Security>().ReverseMap();
            CreateMap<SecurityTypeDto, SecurityType>().ReverseMap();
            CreateMap<BrokerDto, Broker>().ReverseMap();
            CreateMap<BrokerAccountDto, BrokerAccount>().ReverseMap();
            CreateMap<BrokerAccountSecurityDto, BrokerAccountSecurity>().ReverseMap();
            CreateMap<BrokerAccountTypeDto, BrokerAccountType>().ReverseMap();
        }
    }
}