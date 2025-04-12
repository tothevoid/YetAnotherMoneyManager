using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.WEB.Model;
using BLL.DTO;
using MoneyManager.Model.Server;
using MoneyManager.Model.Common;
using MoneyManager.Model.Charts.Deposit;
using MoneyManager.Model.Deposits;
using MoneyManager.DAL.Entities;

namespace MoneyManager.WEB.Mappings
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
