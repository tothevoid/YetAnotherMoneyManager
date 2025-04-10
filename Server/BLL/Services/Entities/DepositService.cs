﻿using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.DAL.Entities;
using MoneyManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Interfaces.Entities;
using DAL.Constants;
using MoneyManager.Model.Server;
using System.Linq.Expressions;

namespace MoneyManager.BLL.Services.Entities
{
    public class DepositService : IDepositService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Deposit> _depositRepo;
        private readonly IRepository<Account> _accountRepo;


        private readonly IMapper _mapper;
        public DepositService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _depositRepo = uow.CreateRepository<Deposit>();
            _accountRepo = uow.CreateRepository<Account>();
        }

        public async Task<IEnumerable<DepositDTO>> GetAll(int monthsFrom, int monthsTo, bool onlyActive)
        {
            var deposits = await GetDeposits(monthsFrom, monthsTo, onlyActive);
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

        public async Task Delete(Guid id)
        {
            await _depositRepo.Delete(id);
            _db.Commit();
        }

        public async Task<DepositMonthSummaryDTO> GetSummary(int monthsFrom, int monthsTo, bool onlyActive)
        {
            //TODO: IEnumerable order => IQueryable order
            //TODO: sort in db
            var deposits = (await GetDeposits(monthsFrom, monthsTo, onlyActive)).OrderBy(deposit => deposit.From).ToList();
            var dates = new Dictionary<DateOnly, List<(string name, decimal value)>>();

            if (!deposits.Any())
            {
                return new DepositMonthSummaryDTO()
                {
                    Deposits = Enumerable.Empty<string>(),
                    Payments = Enumerable.Empty<PeriodPaymentDTO>(),
                };
            }

            foreach (var deposit in deposits)
            {
                var depositDays = deposit.To.DayNumber - deposit.From.DayNumber;
                var periodStartDate = deposit.From;

                while (periodStartDate < deposit.To)
                {
                    var lastMonthDay = new DateOnly(periodStartDate.Year, periodStartDate.Month, 1).AddMonths(1).AddDays(-1);
                    var periodEndDate = deposit.To < lastMonthDay ? deposit.To : lastMonthDay;

                    decimal profit = CalculateProfitInRange(periodStartDate, periodEndDate, depositDays, deposit.EstimatedEarn);

                    var date = new DateOnly(periodStartDate.Year, periodStartDate.Month, 1);
                    if (dates.ContainsKey(date))
                    {
                        dates[date].Add((deposit.Name, profit));
                    }
                    else
                    {
                        dates[date] = new List<(string name, decimal value)>()
                        {
                            (deposit.Name, profit)
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
                            new DepositPaymentDTO
                            {
                                Name = payment.name, 
                                Value = (decimal) Math.Round(payment.value, 2)
                            })
                    })
            };
        }

        public async Task<DepositsRangeDto> GetDepositsRange()
        {
            var minValueRequest = _depositRepo.GetMin((deposit) => deposit.From);
            var maxValueRequest = _depositRepo.GetMax((deposit) => deposit.To);
            await Task.WhenAll(new Task[] {minValueRequest, maxValueRequest});

            var minValue = minValueRequest.Result.From;
            var maxValue = maxValueRequest.Result.To;
            var rangeStart = new DateOnly(minValue.Year, minValue.Month, 1);
            var rangeEnd = new DateOnly(maxValue.Year, maxValue.Month, 1).AddMonths(1).AddDays(-1);

            return new DepositsRangeDto() { From = rangeStart, To = rangeEnd };
        }

        private async Task<IEnumerable<Deposit>> GetDeposits(int monthsFrom, int monthsTo, bool onlyActive)
        {
            int minYear = (monthsFrom - 1) / 12;
            int minMonth = (monthsFrom - 1) % 12 + 1;

            int maxYear = (monthsTo - 1) / 12;
            int maxMonth = (monthsTo - 1) % 12 + 1;

            var rangeMin = new DateOnly(minYear, minMonth, 1);
            var rangeMax = new DateOnly(maxYear, maxMonth, 1).AddMonths(1).AddDays(-1);

            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

            Expression<Func<Deposit, bool>> filter = onlyActive ?
                (deposit) => deposit.To >= rangeMin && deposit.From <= rangeMax && (deposit.To >= currentDate && deposit.From <= currentDate):
                (deposit) => deposit.To >= rangeMin && deposit.From <= rangeMax;

            return await _depositRepo.GetAll(filter);
        }

        private decimal CalculateProfitInRange(DateOnly from, DateOnly to, int totalDays, decimal estimatedEarn)
        {
            var days = to.DayNumber - from.DayNumber;
            return estimatedEarn / totalDays * days;
        }
    }
}