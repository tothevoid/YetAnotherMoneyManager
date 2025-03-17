using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.WEB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using MoneyManager.Model.Server;
using MoneyManager.Model.Common;
using MoneyManager.Model.Charts.Deposit;

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

            CreateMap<DepositMonthSummaryDTO, DepositMonthSummary>();
            CreateMap<DepositPaymentDTO, DepositPayment>();
            CreateMap<PeriodPaymentDTO, PeriodPayment>();
        }
    }
}
