using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.Model.Charts.Deposit;
using MoneyManager.Model.Deposits;
using MoneyManager.WebApi.Models.Deposit;
using MoneyManager.WebApi.Models.Security;
using MoneyManager.WebApi.Models.Broker;
using MoneyManager.WebApi.Models.Account;
using MoneyManager.WebApi.Models.Currency;
using MoneyManager.WebApi.Models.Transaction;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.DTO.Deposits;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Application.DTO.Securities;

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

            CreateMap<DepositsRangeDto, DepositsRangeModel>();
            CreateMap<DepositMonthSummaryDTO, DepositMonthSummary>();
            CreateMap<DepositPaymentDTO, DepositPayment>();
            CreateMap<PeriodPaymentDTO, PeriodPayment>();
            CreateMap<AccountTransferModel, AccountTransferDto>();
            CreateMap<AccountCurrencySummaryDto, AccountCurrencySummaryModel>();

            CreateMap<ClientDepositModel, ClientDepositDto>();
            CreateMap<ServerDepositDto, ServerDepositModel>();

            CreateMap<SecurityModel, SecurityDto>().ReverseMap();
            CreateMap<SecurityTypeModel, SecurityTypeDto>().ReverseMap();
            CreateMap<BrokerModel, BrokerDto>().ReverseMap();
            CreateMap<BrokerAccountModel, BrokerAccountDto>().ReverseMap();
            CreateMap<BrokerAccountSecurityModel, BrokerAccountSecurityDto>().ReverseMap();
            CreateMap<BrokerAccountTypeModel, BrokerAccountTypeDto>().ReverseMap();
        }
    }
}
