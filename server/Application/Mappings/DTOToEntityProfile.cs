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
using MoneyManager.Infrastructure.Entities.User;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.Infrastructure.Entities.Debts;
using MoneyManager.Infrastructure.Entities.Crypto;
using MoneyManager.Application.DTO.Banks;
using MoneyManager.Infrastructure.Entities.Banks;

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
            CreateMap<CurrencyTransactionDto, CurrencyTransaction>().ReverseMap();

            CreateMap<DepositDTO, Deposit>().ReverseMap();

            CreateMap<SecurityDTO, Security>().ReverseMap();
            CreateMap<SecurityTypeDTO, SecurityType>().ReverseMap();
            CreateMap<DividendDto, Dividend>().ReverseMap();
            CreateMap<BrokerDTO, Broker>().ReverseMap();
            CreateMap<BrokerAccountDTO, BrokerAccount>().ReverseMap();
            CreateMap<BrokerAccountSecurityDTO, BrokerAccountSecurity>().ReverseMap();
            CreateMap<BrokerAccountTypeDTO, BrokerAccountType>().ReverseMap();
            CreateMap<SecurityTransactionDTO, SecurityTransaction>().ReverseMap();
            CreateMap<UserProfileDto, UserProfile>().ReverseMap();

            CreateMap<DebtDto, Debt>().ReverseMap();
            CreateMap<DebtPaymentDto, DebtPayment>().ReverseMap();

            CreateMap<DividendPaymentDto, DividendPayment>().ReverseMap();

            CreateMap<CryptoAccountCryptocurrencyDto, CryptoAccountCryptocurrency>().ReverseMap();
            CreateMap<CryptoAccountDto, CryptoAccount>().ReverseMap();
            CreateMap<CryptocurrencyDto, Cryptocurrency>().ReverseMap();
            CreateMap<CryptoProviderDto, CryptoProvider>().ReverseMap();

            CreateMap<BrokerAccountFundsTransferDto, BrokerAccountFundsTransfer>().ReverseMap();
            CreateMap<BankDto, Bank>().ReverseMap();
            CreateMap<BrokerAccountTaxDeductionDto, BrokerAccountTaxDeduction>().ReverseMap();
        }
    }
}