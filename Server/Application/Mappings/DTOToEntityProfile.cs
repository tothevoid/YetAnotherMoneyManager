using AutoMapper;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Application.DTO.Deposits;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.BLL.DTO;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Infrastructure.Entities.Deposits;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Entities.Transactions;
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