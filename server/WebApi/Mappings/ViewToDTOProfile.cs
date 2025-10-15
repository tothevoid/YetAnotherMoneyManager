using AutoMapper;
using MoneyManager.Application.DTO;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.DTO.Deposits;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.WebApi.Models.Accounts;
using MoneyManager.WebApi.Models.Brokers;
using MoneyManager.WebApi.Models.Currencies;
using MoneyManager.WebApi.Models.Deposits;
using MoneyManager.WebApi.Models.Deposits.Charts;
using MoneyManager.WebApi.Models.Securities;
using MoneyManager.WebApi.Models.Transactions;
using MoneyManager.WebApi.Models.User;
using MoneyManager.WebApi.Models.Dashboard;
using MoneyManager.Application.DTO.Dashboard;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.WebApi.Models.Debts;
using MoneyManager.Infrastructure.Entities.Crypto;
using MoneyManager.WebApi.Models.Crypto;

namespace MoneyManager.WebApi.Mappings
{
    public class ViewToDTOProfile : Profile
    {
        public ViewToDTOProfile()
        {
            CreateMap<TransactionModel, TransactionDTO>().ReverseMap();
            CreateMap<AccountModel, AccountDTO>().ReverseMap();
            CreateMap<TransactionTypeModel, TransactionTypeDTO>().ReverseMap();
            CreateMap<DepositModel, DepositDTO>().ReverseMap();
            CreateMap<CurrencyModel, CurrencyDTO>().ReverseMap();
            CreateMap<AccountTypeModel, AccountTypeDTO>().ReverseMap();

            CreateMap<DepositsRangeDTO, DepositsRangeModel>();
            CreateMap<DepositMonthSummaryDTO, DepositMonthSummary>();
            CreateMap<DepositPaymentDTO, DepositPayment>();
            CreateMap<PeriodPaymentDTO, PeriodPayment>();
            CreateMap<AccountTransferModel, AccountTransferDTO>();
            CreateMap<AccountCurrencySummaryDTO, AccountCurrencySummaryModel>();

            CreateMap<DepositModel, DepositDTO>();

            CreateMap<SecurityModel, SecurityDTO>().ReverseMap();
            CreateMap<DividendModel, DividendDto>().ReverseMap();
            CreateMap<SecurityTypeModel, SecurityTypeDTO>().ReverseMap();
            CreateMap<BrokerModel, BrokerDTO>().ReverseMap();
            CreateMap<BrokerAccountModel, BrokerAccountDTO>().ReverseMap();
            CreateMap<BrokerAccountSecurityModel, BrokerAccountSecurityDTO>().ReverseMap();
            CreateMap<BrokerAccountTypeModel, BrokerAccountTypeDTO>().ReverseMap();
            CreateMap<SecurityTransactionModel, SecurityTransactionDTO>().ReverseMap();
            CreateMap<SecurityTransactionPaginationDto, SecurityTransactionPaginationModel>();
            CreateMap<SecurityHistoryValueDto, SecurityHistoryValueModel>().ReverseMap();

            CreateMap<UserProfileModel, UserProfileDto>().ReverseMap();

            CreateMap<DashboardDto, DashboardModel>();
            CreateMap<TransactionStatsDto, TransactionStatsModel>();
            CreateMap<BrokerAccountStatsDto, BrokerAccountStatsModel>();
            CreateMap<AccountStatsDto, AccountStatsModel>();
            CreateMap<DistributionDto, DistributionModel>().ReverseMap();
            CreateMap<DebtStatsDto, DebtStatsModel>().ReverseMap();
            CreateMap<DepositStatsModel, DepositStats>().ReverseMap();
            CreateMap<CryptoAccountStatsDto, CryptoAccountStatsModel>().ReverseMap();

            CreateMap<SecurityTransactionsHistoryDto, SecurityTransactionsHistoryModel>();

            CreateMap<DebtDto, DebtModel>().ReverseMap();
            CreateMap<DebtPaymentDto, DebtPaymentModel>().ReverseMap();

            CreateMap<DividendPaymentDto, DividendPaymentModel>().ReverseMap();
            CreateMap<SecurityStatsDto, SecurityStatsModel>().ReverseMap();

            CreateMap<CryptoAccountCryptocurrencyDto, CryptoAccountCryptocurrencyModel>().ReverseMap();
            CreateMap<CryptoAccountDto, CryptoAccountModel>().ReverseMap();
            CreateMap<CryptocurrencyDto, CryptocurrencyModel>().ReverseMap();
            CreateMap<CryptoProviderDto, CryptoProviderModel>().ReverseMap();

            CreateMap<BrokerAccountFundsTransferDto, BrokerAccountFundsTransferModel>().ReverseMap();

            CreateMap<BrokerAccountSummaryDto, BrokerAccountSummaryModel>().ReverseMap();
            CreateMap<DailyStatDto, DailyStatModel>().ReverseMap();
            
        }
    }
}
