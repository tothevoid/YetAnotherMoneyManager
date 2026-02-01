using ClosedXML.Excel;
using MoneyManager.Application.DTO.Banks;
using MoneyManager.Application.DTO.Dashboard;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Application.Interfaces.Banks;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Currencies;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.Application.Interfaces.Deposits;
using MoneyManager.Application.Interfaces.Reports;
using MoneyManager.Application.Services.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Reports
{
    // TODO: add localization
    // TODO: dynamic headers

    public class AllAssetsReportService: IAllAssetsReportService
    {
        private readonly IDepositService _depositService;
        private readonly IAccountService _accountService;
        private readonly IBankService _bankService;
        private readonly IBrokerAccountService _brokerAccountService;
        private readonly IBrokerAccountSecurityService _brokerAccountSecurityService;
        private readonly IDebtService _debtService;
        private readonly IDashboardService _dashboardService;

        public AllAssetsReportService(IDepositService depositService, 
            IAccountService accountService, IBankService bankService, 
            IBrokerAccountService brokerAccountService, 
            IBrokerAccountSecurityService brokerAccountSecurityService,
            IDebtService debtService,
            IDashboardService dashboardService)
        {
            _depositService = depositService;
            _accountService = accountService;
            _bankService = bankService;
            _brokerAccountService = brokerAccountService;
            _brokerAccountSecurityService = brokerAccountSecurityService;
            _debtService = debtService;
            _dashboardService = dashboardService;
        }

        public async Task<byte[]> CreateReport()
        {
            using var workbook = new XLWorkbook();

            var totalsSheet = workbook.Worksheets.Add("Итоги");
            await CreateTotalsWorksheet(totalsSheet);
            totalsSheet.Columns().AdjustToContents();

            var bankAccounts = await _bankService.GetAll();
            foreach (var bank in bankAccounts)
            {
                await CreateBankAccountWorksheet(workbook, bank);
            }

            var brokerSheet = workbook.Worksheets.Add("Инвестиции");
            await CreateBrokerAccountWorksheet(brokerSheet);
            brokerSheet.Columns().AdjustToContents();

            var debtorsSheet = workbook.Worksheets.Add("Должники");
            await CreateDebtorsWorksheet(debtorsSheet);
            debtorsSheet.Columns().AdjustToContents();

            using var ms = new System.IO.MemoryStream();
            workbook.SaveAs(ms);
            return ms.ToArray();
        }

        private async Task CreateBankAccountWorksheet(IXLWorkbook workbook, BankDto bank)
        {
            var accounts = (await _accountService.GetAll(true))
                .Where(account => account.BankId == bank.Id).ToList();

            var deposits = (await _depositService.GetAllActive())
                .Where(deposit => deposit.BankId == bank.Id).ToList();

            if (!accounts.Any() || !deposits.Any())
            {
                return;
            }

            var worksheet = workbook.Worksheets.Add(bank.Name);

            worksheet.Cell("A1").Value = $"На счете '{bank.Name}'";
            worksheet.Cell("B1").Value = "Количество";
            worksheet.Cell("C1").Value = "Отношение к осн. валюте";
            worksheet.Cell("D1").Value = "Процент";
            worksheet.Cell("E1").Value = "Дата начала";
            worksheet.Cell("F1").Value = "Кол-во дней";
            worksheet.Cell("G1").Value = "Доход";

            int reservedRowsBeforeSummaries = 10;

            decimal total = 0;
            decimal totalDynamic = 0;
            decimal totalStatic = 0;
            decimal incomeInMonth = 0;
            decimal notConfirmedIncomes = 0;

            int currentRow = 2;

            foreach (var account in accounts)
            {
                worksheet.Cell($"A{currentRow}").Value = account.Name;
                SetFinanceValue(worksheet.Cell($"B{currentRow}"), account.Balance);
                SetFinanceValue(worksheet.Cell($"C{currentRow}"), account.Currency.Rate);

                totalStatic += account.Balance * account.Currency.Rate;
                total += account.Balance * account.Currency.Rate;

                currentRow++;
            }

            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            foreach (var deposit in deposits)
            {
                var income = deposit.InitialAmount / 365 / 100 * deposit.Percentage *
                             (currentDate.DayNumber - deposit.From.DayNumber);

                int totalDays = deposit.To.DayNumber - deposit.From.DayNumber;
                int daysPassed = currentDate.DayNumber - deposit.From.DayNumber;

                worksheet.Cell($"A{currentRow}").Value = deposit.Name;
                SetFinanceValue(worksheet.Cell($"B{currentRow}"), deposit.InitialAmount);
                SetFinanceValue(worksheet.Cell($"C{currentRow}"), deposit.Currency.Rate);
                worksheet.Cell($"D{currentRow}").Value = deposit.Percentage;
                worksheet.Cell($"E{currentRow}").Value = deposit.From.ToDateTime(new TimeOnly());
                worksheet.Cell($"F{currentRow}").Value = totalDays;
                SetFinanceValue(worksheet.Cell($"G{currentRow}"), deposit.EstimatedEarn / totalDays * daysPassed);

                total += deposit.InitialAmount + income;
                totalDynamic += deposit.InitialAmount;
                notConfirmedIncomes += income;
                incomeInMonth += deposit.InitialAmount / 12 / 100 * deposit.Percentage;

                currentRow++;
            }

            worksheet.Cell($"A{reservedRowsBeforeSummaries}").Value = "Итого";
            SetFinanceValue(worksheet.Cell($"B{reservedRowsBeforeSummaries++}"), total);

            worksheet.Cell($"A{reservedRowsBeforeSummaries}").Value = "Итого динамических";
            SetFinanceValue(worksheet.Cell($"B{reservedRowsBeforeSummaries++}"), totalDynamic);

            worksheet.Cell($"A{reservedRowsBeforeSummaries}").Value = "В месяц";
            SetFinanceValue(worksheet.Cell($"B{reservedRowsBeforeSummaries++}"), incomeInMonth);

            worksheet.Cell($"A{reservedRowsBeforeSummaries}").Value = "Только после завершения";
            SetFinanceValue(worksheet.Cell($"B{reservedRowsBeforeSummaries++}"), notConfirmedIncomes);

            worksheet.Cell($"A{reservedRowsBeforeSummaries}").Value = "Итого статических";
            SetFinanceValue(worksheet.Cell($"B{reservedRowsBeforeSummaries}"), totalStatic);

            worksheet.Columns().AdjustToContents();
        }

        private async Task CreateBrokerAccountWorksheet(IXLWorksheet worksheet)
        {
            var accounts = await _brokerAccountService.GetAll();

            worksheet.Cell("A1").Value = "Тикер";
            worksheet.Cell("B1").Value = "Количество";
            worksheet.Cell("C1").Value = "Цена";
            worksheet.Cell("D1").Value = "Итого";

            var currentRow = 2;

            var accountsByCurrency = accounts.GroupBy(account => account.CurrencyId);

            foreach (var accountsByCurrencyGroup in accountsByCurrency)
            {
                var amount = accountsByCurrencyGroup.Sum(account => account.MainCurrencyAmount);

                var currency = accountsByCurrencyGroup.First().Currency;

                worksheet.Cell($"A{currentRow}").Value = currency.Name;
                SetFinanceValue(worksheet.Cell($"B{currentRow}"), amount);
                SetFinanceValue(worksheet.Cell($"C{currentRow}"), currency.Rate);
                SetFinanceValue(worksheet.Cell($"D{currentRow}"), currency.Rate * amount);

                currentRow++;
            }

            var brokerAccountSecurities = await _brokerAccountSecurityService.GetAll(true);

            foreach (var brokerAccountSecurity in brokerAccountSecurities)
            {
                worksheet.Cell($"A{currentRow}").Value = brokerAccountSecurity.Security.Ticker;
                SetFinanceValue(worksheet.Cell($"B{currentRow}"), brokerAccountSecurity.Quantity);
                SetFinanceValue(worksheet.Cell($"C{currentRow}"), brokerAccountSecurity.Security.ActualPrice);
                SetFinanceValue(worksheet.Cell($"D{currentRow}"), brokerAccountSecurity.Security.ActualPrice * brokerAccountSecurity.Quantity);

                currentRow++;
            }
        }

        private async Task CreateDebtorsWorksheet(IXLWorksheet worksheet)
        {
            worksheet.Cell("A1").Value = "Название";
            worksheet.Cell("B1").Value = "Количество";
            worksheet.Cell("C1").Value = "Отношение к осн. валюте";

            var activeDebtors = await _debtService.GetAll(true);

            int currentRow = 2;
            decimal total = 0;

            foreach (var debt in activeDebtors)
            {
                worksheet.Cell($"A{currentRow}").Value = debt.Name;
                SetFinanceValue(worksheet.Cell($"B{currentRow}"), debt.Amount);
                SetFinanceValue(worksheet.Cell($"C{currentRow}"), debt.Currency.Rate);

                total += debt.Currency.Rate * debt.Amount;

                currentRow++;
            }

            int totalsRow = 20;
            worksheet.Cell($"A{totalsRow}").Value = "Итого:";
            SetFinanceValue(worksheet.Cell($"B{totalsRow}"), total);
        }

        private async Task CreateTotalsWorksheet(IXLWorksheet worksheet)
        {
            var dashboard = await _dashboardService.GetDashboard();

            var currentRow = 1;

            foreach (var cash in dashboard.AccountsGlobalDashboard.CashDistribution)
            {
                worksheet.Cell($"A{currentRow}").Value = $"В физических '{cash.Name}'";
                SetFinanceValue(worksheet.Cell($"B{currentRow}"), cash.ConvertedAmount);
                SetPercentValue(worksheet.Cell($"C{currentRow}"), dashboard.Total == 0 ? 0 : cash.ConvertedAmount / dashboard.Total);
                currentRow++;
            }

            worksheet.Cell($"A{currentRow}").Value = "На баковских счетах";
            SetFinanceValue(worksheet.Cell($"B{currentRow}"), dashboard.AccountsGlobalDashboard.TotalBankAccount);
            SetPercentValue(worksheet.Cell($"C{currentRow}"), dashboard.Total == 0 ? 0 : dashboard.AccountsGlobalDashboard.TotalBankAccount / dashboard.Total);
            currentRow++;

            worksheet.Cell($"A{currentRow}").Value = "В криптовалюте";
            SetFinanceValue(worksheet.Cell($"B{currentRow}"), dashboard.CryptoAccountsGlobalDashboard.Total);
            SetPercentValue(worksheet.Cell($"C{currentRow}"), dashboard.Total == 0 ? 0 : dashboard.CryptoAccountsGlobalDashboard.Total / dashboard.Total);
            currentRow++;

            worksheet.Cell($"A{currentRow}").Value = "Одолжено";
            SetFinanceValue(worksheet.Cell($"B{currentRow}"), dashboard.DebtsGlobalDashboard.Total);
            SetPercentValue(worksheet.Cell($"C{currentRow}"), dashboard.Total == 0 ? 0 : dashboard.DebtsGlobalDashboard.Total / dashboard.Total);
            currentRow++;

            worksheet.Cell($"A{currentRow}").Value = "Инвестировано";
            SetFinanceValue(worksheet.Cell($"B{currentRow}"), dashboard.BrokerAccountsGlobalDashboard.Total);
            SetPercentValue(worksheet.Cell($"C{currentRow}"), dashboard.Total == 0 ? 0 : dashboard.BrokerAccountsGlobalDashboard.Total / dashboard.Total);
            currentRow++;

            worksheet.Cell($"A{currentRow}").Value = "На депозитах";
            SetFinanceValue(worksheet.Cell($"B{currentRow}"), dashboard.DepositsGlobalDashboard.TotalStartedAmount);
            SetPercentValue(worksheet.Cell($"C{currentRow}"), dashboard.Total == 0 ? 0 : dashboard.DepositsGlobalDashboard.TotalStartedAmount / dashboard.Total);
            currentRow++;

            worksheet.Cell($"A{currentRow}").Value = "При завершении депозита";
            SetFinanceValue(worksheet.Cell($"B{currentRow}"), dashboard.DepositsGlobalDashboard.TotalEarned);
            SetPercentValue(worksheet.Cell($"C{currentRow}"), dashboard.Total == 0 ? 0 : dashboard.DepositsGlobalDashboard.TotalEarned / dashboard.Total);
            currentRow++;

            worksheet.Cell($"A{currentRow}").Value = "Итого";
            SetFinanceValue(worksheet.Cell($"B{currentRow}"), dashboard.Total);

            currentRow += 2;

            var currencyDistributions = GetCurrencyDistributions(dashboard);

            foreach (var currency in currencyDistributions)
            {
                worksheet.Cell($"A{currentRow}").Value = currency.Key;
                SetFinanceValue(worksheet.Cell($"B{currentRow}"), currency.Value);
                currentRow++;
            }
        }

        private Dictionary<string, decimal> GetCurrencyDistributions(GlobalDashboardDto dashboard)
        {
            var totalCurrencies = new Dictionary<string, decimal>();

            var summaryDistributions = new List<DistributionDto>();

            summaryDistributions.AddRange(dashboard.AccountsGlobalDashboard.BankAccountsDistribution);
            summaryDistributions.AddRange(dashboard.AccountsGlobalDashboard.CashDistribution);

            summaryDistributions.AddRange(dashboard.BrokerAccountsGlobalDashboard.Distribution);
            summaryDistributions.AddRange(dashboard.CryptoAccountsGlobalDashboard.Distribution);
            summaryDistributions.AddRange(dashboard.DebtsGlobalDashboard.Distribution);
            summaryDistributions.AddRange(dashboard.DepositsGlobalDashboard.EarningsDistribution);
            summaryDistributions.AddRange(dashboard.DepositsGlobalDashboard.StartedAmountDistribution);

            foreach (var distribution in summaryDistributions)
            {
                if (totalCurrencies.ContainsKey(distribution.Currency))
                {
                    totalCurrencies[distribution.Currency] += distribution.Amount;
                }
                else
                {
                    totalCurrencies.Add(distribution.Currency, distribution.Amount);
                }
            }

            return totalCurrencies;
        }

        private void SetPercentValue(IXLCell cell, decimal value)
        {
            cell.Value = value;
            cell.Style.NumberFormat.Format = "0.00%";
        }

        private void SetFinanceValue(IXLCell cell, decimal value)
        {
            cell.Value = value;
            cell.Style.NumberFormat.Format = "#,##0.00";
        }
    }
}
