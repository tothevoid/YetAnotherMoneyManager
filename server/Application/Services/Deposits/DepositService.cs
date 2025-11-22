using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Deposits;
using MoneyManager.Application.Interfaces.Deposits;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Entities.Deposits;
using MoneyManager.Infrastructure.Queries;

namespace MoneyManager.Application.Services.Deposits
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

        public async Task<IEnumerable<DepositDTO>> GetAll(int monthsFrom, int monthsTo, bool onlyActive)
        {
            var deposits = await GetDeposits(monthsFrom, monthsTo, onlyActive, x => x.To, true);
            return _mapper.Map<IEnumerable<DepositDTO>>(deposits);
        }

        public async Task<IEnumerable<DepositDTO>> GetAllActive()
        {
            var deposits = await _depositRepo.GetAll(deposit => deposit.To > DateOnly.FromDateTime(DateTime.Now), 
                include: GetFullHierarchyColumns);
            return _mapper.Map<IEnumerable<DepositDTO>>(deposits.OrderByDescending(x => x.From));
        }

        public async Task<Guid> Add(DepositDTO deposit)
        {
            var mappedDeposit = _mapper.Map<Deposit>(deposit);
            mappedDeposit.Id = Guid.NewGuid();
            await _depositRepo.Add(mappedDeposit);
            await _db.Commit();
            return mappedDeposit.Id;
        }

        public async Task Update(DepositDTO modifiedDeposit)
        {
            var currentDeposit = await _depositRepo.GetById(modifiedDeposit.Id, GetFullHierarchyColumns);

            if (currentDeposit == null)
            {
                return;
            }

            var deposit = _mapper.Map<Deposit>(modifiedDeposit);
            _depositRepo.Update(deposit);
            await _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            await _depositRepo.Delete(id);
            await _db.Commit();
        }

        public async Task<DepositMonthSummaryDTO> GetSummary(int monthsFrom, int monthsTo, bool onlyActive)
        {
            var deposits = (await GetDeposits(monthsFrom, monthsTo, onlyActive, deposit => deposit.From)).ToList();
            var dates = new Dictionary<DateOnly, List<Payment>>();

            if (!deposits.Any())
            {
                return new DepositMonthSummaryDTO()
                {
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
                    var paymentName = deposit.Bank?.Name ?? "";

                    if (dates.ContainsKey(date))
                    {
                        dates[date].Add(new Payment() { DepositId = deposit.Id, Name = paymentName, Value = profit});
                    }
                    else
                    {
                        dates[date] = new List<Payment>()
                        {
                            new(){DepositId = deposit.Id, Name = paymentName, Value = profit}
                        };
                    }

                    periodStartDate = new DateOnly(periodStartDate.Year, periodStartDate.Month, 1).AddMonths(1);
                }
            }


            return new DepositMonthSummaryDTO()
            {
                Payments = dates.Select(date =>
                    new PeriodPaymentDTO
                    {
                        Period = date.Key.ToString("MM.yy"),
                        Payments = date.Value.Select(payment =>
                            new DepositPaymentDTO
                            {
                                DepositId = payment.DepositId,
                                Name = payment.Name, 
                                Value =  Math.Round(payment.Value, 2)
                            })
                    })
            };
        }

        public async Task<DepositsRangeDTO> GetDepositsRange()
        {
            var minValueEntity = await _depositRepo.GetMin((deposit) => deposit.From);
            var maxValueEntity = await _depositRepo.GetMax((deposit) => deposit.To);

            if (minValueEntity == null || maxValueEntity == null)
            {
                return null;
            }
            
            var minValue = minValueEntity.From;
            var maxValue = maxValueEntity.To;
            var rangeStart = new DateOnly(minValue.Year, minValue.Month, 1);
            var rangeEnd = new DateOnly(maxValue.Year, maxValue.Month, 1).AddMonths(1).AddDays(-1);

            return new DepositsRangeDTO() { From = rangeStart, To = rangeEnd };
        }

        private async Task<IEnumerable<Deposit>> GetDeposits(int monthsFrom, int monthsTo, bool onlyActive, 
            Expression<Func<Deposit, object>> orderBy, bool isDescending = false)
        {
            int minYear = (monthsFrom - 1) / 12;
            int minMonth = (monthsFrom - 1) % 12 + 1;

            int maxYear = (monthsTo - 1) / 12;
            int maxMonth = (monthsTo - 1) % 12 + 1;

            var rangeMin = new DateOnly(minYear, minMonth, 1);
            var rangeMax = new DateOnly(maxYear, maxMonth, 1).AddMonths(1).AddDays(-1);

            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

            Expression<Func<Deposit, bool>> filter = onlyActive ?
                (deposit) => deposit.To >= rangeMin && deposit.From <= rangeMax && deposit.To >= currentDate:
                (deposit) => deposit.To >= rangeMin && deposit.From <= rangeMax;

            var complexQuery = new ComplexQueryBuilder<Deposit>()
                .AddFilter(filter)
                .AddJoins(GetFullHierarchyColumns)
                .AddOrder(orderBy, isDescending)
                .DisableTracking();

            return await _depositRepo.GetAll(complexQuery.GetQuery());
        }

        private decimal CalculateProfitInRange(DateOnly from, DateOnly to, int totalDays, decimal estimatedEarn)
        {
            var days = to.DayNumber - from.DayNumber;
            return estimatedEarn / totalDays * days;
        }

        private IQueryable<Deposit> GetFullHierarchyColumns(IQueryable<Deposit> depositQuery)
        {
            return depositQuery
                .Include(deposit => deposit.Currency)
                .Include(deposit => deposit.Bank);
        }

        private class Payment
        {
            public Guid DepositId { get; set; }

            public string Name { get; set; }

            public decimal Value { get; set; }

            public Guid? BankId { get; set; }
        }
    }
}