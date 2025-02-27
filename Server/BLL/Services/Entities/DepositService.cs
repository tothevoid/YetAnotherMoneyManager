using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.DAL.Entities;
using MoneyManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Interfaces.Entities;
using MoneyManager.Model.Server;

namespace MoneyManager.BLL.Services.Entities
{
    public class DepositService : IDepositService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Deposit> _depositRepo;
        private readonly IMapper _mapper;
        public DepositService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _depositRepo = uow.CreateRepository<Deposit>();
        }

        public async Task<IEnumerable<DepositDTO>> GetAll()
        {
            var deposits = await _depositRepo.GetAll();
            return _mapper.Map<IEnumerable<DepositDTO>>(deposits.OrderByDescending(x => x.From));
        }

        public async Task<Guid> Add(DepositDTO deposit)
        {
            var mappedDeposit = _mapper.Map<Deposit>(deposit);
            mappedDeposit.Id = Guid.NewGuid();
            await _depositRepo.Add(mappedDeposit);
            _db.Commit();
            return mappedDeposit.Id;
        }

        public async Task Update(DepositDTO updateTransactionModel)
        {
            var deposit = _mapper.Map<Deposit>(updateTransactionModel);
            await _depositRepo.Update(deposit);
            _db.Commit();
        }

        public async Task Delete(DepositDTO deposit)
        {
            await _depositRepo.Delete(deposit.Id);
            _db.Commit();
        }

        public async Task<DepositMonthSummaryDTO> GetSummary()
        {
            var deposits = (await _depositRepo.GetAll()).ToList();

            var dates = new Dictionary<DateOnly, List<(string name, decimal value)>>();

            foreach (var deposit in deposits)
            {
                var accumulator = deposit.InitialAmount;
                var periodStartDate = DateOnly.FromDateTime(deposit.From);
                var depositEndDate = DateOnly.FromDateTime(deposit.To);

                while (periodStartDate < depositEndDate)
                {
                    var lastMonthDay = new DateOnly(periodStartDate.Year, periodStartDate.Month, 1).AddMonths(1).AddDays(-1);
                    var periodEndDate = depositEndDate < lastMonthDay ? depositEndDate : lastMonthDay;

                    var days = periodEndDate.DayNumber - periodStartDate.DayNumber;
                    decimal income = accumulator / 365 * days / 100 * (decimal) deposit.Percentage;
                    accumulator += income;

                    var date = new DateOnly(periodStartDate.Year, periodStartDate.Month, 1);
                    if (dates.ContainsKey(date))
                    {
                        dates[date].Add((deposit.Name, income));
                    }
                    else
                    {
                        dates[date] = new List<(string name, decimal value)>()
                        {
                            (deposit.Name, income)
                        };
                    }

                    periodStartDate = new DateOnly(periodStartDate.Year, periodStartDate.Month, 1).AddMonths(1);
                }
            }


            return new DepositMonthSummaryDTO()
            {
                Deposits = deposits.Select(deposit => deposit.Name).ToList(),
                Payments = dates.Select(date =>
                    new PeriodPaymentDTO
                    {
                        Period = date.Key.ToString("MM.yy"),
                        Payments = date.Value.Select(payment =>
                            new DepositPaymentDTO { Name = payment.name, Value = payment.value })
                    })
            };
        }
    }
}