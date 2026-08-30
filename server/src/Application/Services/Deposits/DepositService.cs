using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Audex.Application.DTO.Deposits;
using Audex.Application.Interfaces.Deposits;
using Audex.Application.Mappings;
using Audex.Infrastructure.Interfaces.Database;
using Audex.Infrastructure.Entities.Deposits;
using Audex.Infrastructure.Queries;

namespace Audex.Application.Services.Deposits
{
    public class DepositService : IDepositService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Deposit> _depositRepo;

        private readonly ApplicationMapper _mapper;
        public DepositService(IUnitOfWork uow, ApplicationMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _depositRepo = uow.CreateRepository<Deposit>();
        }

        public async Task<IEnumerable<DepositDto>> GetAllAsync(int monthsFrom, int monthsTo, bool onlyActive)
        {
            var deposits = await GetDeposits(monthsFrom, monthsTo, onlyActive, x => x.To, false);
            return _mapper.Map(deposits);
        }

        public async Task<IEnumerable<DepositDto>> GetAllActiveAsync()
        {
            var deposits = await _depositRepo.GetAllAsync(deposit => deposit.To > DateOnly.FromDateTime(DateTime.Now), 
                include: GetFullHierarchyColumns);
            return _mapper.Map(deposits.OrderByDescending(x => x.From));
        }

        public async Task<Guid> AddAsync(DepositDto deposit)
        {
            var mappedDeposit = _mapper.Map(deposit);
            mappedDeposit.Id = Guid.NewGuid();
            await _depositRepo.AddAsync(mappedDeposit);
            await _db.CommitAsync();
            return mappedDeposit.Id;
        }

        public async Task UpdateAsync(DepositDto modifiedDeposit)
        {
            var currentDeposit = await _depositRepo.GetByIdAsync(modifiedDeposit.Id, GetFullHierarchyColumns);

            if (currentDeposit == null)
            {
                return;
            }

            var deposit = _mapper.Map(modifiedDeposit);
            _depositRepo.Update(deposit);
            await _db.CommitAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _depositRepo.DeleteAsync(id);
            await _db.CommitAsync();
        }

        public async Task<DepositMonthSummaryDto> GetSummaryAsync(int monthsFrom, int monthsTo, bool onlyActive)
        {
            var deposits = (await GetDeposits(monthsFrom, monthsTo, onlyActive, deposit => deposit.From)).ToList();
            if (!deposits.Any())
            {
                return CreateEmptySummary();
            }

            var periodPayments = CalculateMonthlyPayments(deposits);
            var totalEarnings = Math.Round(periodPayments.Sum(p => p.TotalValue), 2);
            var monthsCount = periodPayments.Count;
            var avgMonthly = monthsCount > 0 ? Math.Round(totalEarnings / monthsCount, 2) : 0m;
            var peak = periodPayments.OrderByDescending(p => p.TotalValue).FirstOrDefault();

            return new DepositMonthSummaryDto
            {
                TotalEarnings = totalEarnings,
                AverageMonthly = avgMonthly,
                PeakMonthPeriod = peak?.Period,
                PeakMonthValue = peak?.TotalValue ?? 0m,
                MonthsCount = monthsCount,
                DepositTotals = CalculateDepositTotals(periodPayments),
                Payments = periodPayments
            };
        }

        private static DepositMonthSummaryDto CreateEmptySummary() => new()
        {
            TotalEarnings = 0m,
            AverageMonthly = 0m,
            PeakMonthPeriod = null,
            PeakMonthValue = 0m,
            MonthsCount = 0,
            DepositTotals = Enumerable.Empty<DepositSummaryItemDto>(),
            Payments = Enumerable.Empty<PeriodPaymentDto>(),
        };

        private List<PeriodPaymentDto> CalculateMonthlyPayments(IEnumerable<Deposit> deposits)
        {
            var monthlyPayments = GroupMonthlyPayments(deposits);
            return MapToPeriodPayments(monthlyPayments);
        }

        private Dictionary<DateOnly, List<Payment>> GroupMonthlyPayments(IEnumerable<Deposit> deposits)
        {
            var dates = new Dictionary<DateOnly, List<Payment>>();

            foreach (var deposit in deposits)
            {
                DistributeDepositPayments(deposit, dates);
            }

            return dates;
        }

        private void DistributeDepositPayments(Deposit deposit, Dictionary<DateOnly, List<Payment>> dates)
        {
            var depositDays = deposit.To.DayNumber - deposit.From.DayNumber;
            var periodStartDate = deposit.From;
            var paymentName = !string.IsNullOrWhiteSpace(deposit.Bank?.Name) ? deposit.Bank.Name : deposit.Name;

            while (periodStartDate < deposit.To)
            {
                var lastMonthDay = new DateOnly(periodStartDate.Year, periodStartDate.Month, 1).AddMonths(1).AddDays(-1);
                var periodEndDate = deposit.To < lastMonthDay ? deposit.To : lastMonthDay;

                decimal profit = CalculateProfitInRange(periodStartDate, periodEndDate, depositDays, deposit.EstimatedEarn);
                var date = new DateOnly(periodStartDate.Year, periodStartDate.Month, 1);

                if (!dates.TryGetValue(date, out var list))
                {
                    list = new List<Payment>();
                    dates[date] = list;
                }

                list.Add(new Payment { DepositId = deposit.Id, Name = paymentName, Value = profit });
                periodStartDate = new DateOnly(periodStartDate.Year, periodStartDate.Month, 1).AddMonths(1);
            }
        }

        private static List<PeriodPaymentDto> MapToPeriodPayments(Dictionary<DateOnly, List<Payment>> dates)
        {
            return dates.OrderBy(d => d.Key).Select(date =>
            {
                var paymentsList = date.Value.Select(payment => new DepositPaymentDto
                {
                    DepositId = payment.DepositId,
                    Name = payment.Name,
                    Value = Math.Round(payment.Value, 2)
                }).ToList();

                return new PeriodPaymentDto
                {
                    Period = date.Key,
                    TotalValue = Math.Round(paymentsList.Sum(p => p.Value), 2),
                    Payments = paymentsList
                };
            }).ToList();
        }

        private static List<DepositSummaryItemDto> CalculateDepositTotals(IEnumerable<PeriodPaymentDto> periodPayments)
        {
            return periodPayments
                .SelectMany(p => p.Payments)
                .GroupBy(p => p.DepositId)
                .Select(g => new DepositSummaryItemDto
                {
                    DepositId = g.Key,
                    Name = g.First().Name,
                    TotalValue = Math.Round(g.Sum(p => p.Value), 2)
                })
                .OrderByDescending(d => d.TotalValue)
                .ToList();
        }

        public async Task<DepositsRangeDto> GetDepositsRangeAsync()
        {
            var minValueEntity = await _depositRepo.GetMinAsync((deposit) => deposit.From);
            var maxValueEntity = await _depositRepo.GetMaxAsync((deposit) => deposit.To);

            if (minValueEntity == null || maxValueEntity == null)
            {
                return null;
            }
            
            var minValue = minValueEntity.From;
            var maxValue = maxValueEntity.To;
            var rangeStart = new DateOnly(minValue.Year, minValue.Month, 1);
            var rangeEnd = new DateOnly(maxValue.Year, maxValue.Month, 1).AddMonths(1).AddDays(-1);

            return new DepositsRangeDto() { From = rangeStart, To = rangeEnd };
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
                .AddOrder(orderBy, isDescending);

            return await _depositRepo.GetAllAsync(complexQuery.GetQuery());
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