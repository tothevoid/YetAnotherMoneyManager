using AutoMapper;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Application.DTO.Deposits;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.DTO;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Infrastructure.Entities.Deposits;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Entities.Transactions;
using MoneyManager.Model.Deposits;

namespace MoneyManager.Application.Mappings
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

            CreateMap<Deposit, ServerDepositDTO>().ReverseMap();
            CreateMap<ClientDepositDTO, Deposit>().ReverseMap();

            CreateMap<SecurityDTO, Security>().ReverseMap();
            CreateMap<SecurityTypeDTO, SecurityType>().ReverseMap();
            CreateMap<BrokerDTO, Broker>().ReverseMap();
            CreateMap<BrokerAccountDTO, BrokerAccount>().ReverseMap();
            CreateMap<BrokerAccountSecurityDTO, BrokerAccountSecurity>().ReverseMap();
            CreateMap<BrokerAccountTypeDTO, BrokerAccountType>().ReverseMap();
            CreateMap<SecurityTransactionDTO, SecurityTransaction>().ReverseMap();
        }
    }
}