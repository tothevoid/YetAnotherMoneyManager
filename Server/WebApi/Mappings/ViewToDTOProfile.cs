using AutoMapper;
using MoneyManager.Application.DTO;
using MoneyManager.Model.Deposits;
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

namespace MoneyManager.WebApi.Mappings
{
    public class ViewToDTOProfile : Profile
    {
        public ViewToDTOProfile()
        {
            CreateMap<TransactionModel, TransactionDTO>().ReverseMap();
            CreateMap<AccountModel, AccountDTO>().ReverseMap();
            CreateMap<UpdateAccountModel, UpdateAccountDTO>().ReverseMap();
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

            CreateMap<ClientDepositModel, ClientDepositDTO>();
            CreateMap<ServerDepositDTO, ServerDepositModel>();

            CreateMap<SecurityModel, SecurityDTO>().ReverseMap();
            CreateMap<SecurityTypeModel, SecurityTypeDTO>().ReverseMap();
            CreateMap<BrokerModel, BrokerDTO>().ReverseMap();
            CreateMap<BrokerAccountModel, BrokerAccountDTO>().ReverseMap();
            CreateMap<BrokerAccountSecurityModel, BrokerAccountSecurityDTO>().ReverseMap();
            CreateMap<BrokerAccountTypeModel, BrokerAccountTypeDTO>().ReverseMap();
            CreateMap<SecurityTransactionModel, SecurityTransactionDTO>().ReverseMap();
            CreateMap<BrokerAccountSecurityPaginationDto, BrokerAccountSecurityPaginationModel>();
        }
    }
}
