using ClosedXML.Excel;
using MoneyManager.Application.DTO.Banks;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Application.Interfaces.Banks;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.Application.Interfaces.Deposits;
using MoneyManager.Application.Interfaces.Reports;
using System;
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

        public AllAssetsReportService(IDepositService depositService, 
            IAccountService accountService, IBankService bankService, 
            IBrokerAccountService brokerAccountService, 
            IBrokerAccountSecurityService brokerAccountSecurityService,
            IDebtService debtService)
        {
            _depositService = depositService;
            _accountService = accountService;
            _bankService = bankService;
            _brokerAccountService = brokerAccountService;
            _brokerAccountSecurityService = brokerAccountSecurityService;
            _debtService = debtService;
        }

        public async Task<byte[]> CreateReport()
        {
            using var workbook = new XLWorkbook();

            var bankAccounts = await _bankService.GetAll();
            foreach (var bank in bankAccounts)
            {
                await CreateBankAccountWorksheet(workbook, bank);
            }

            var brokerSheet = workbook.Worksheets.Add("Брокерские счета");
            await CreateBrokerAccountWorksheet(brokerSheet);
            brokerSheet.Columns().AdjustToContents();

            var debtorsSheet = workbook.Worksheets.Add("Должники");
            await CreateDebtorsWorksheet(debtorsSheet);
            debtorsSheet.Columns().AdjustToContents();

            using var ms = new System.IO.MemoryStream();
            workbook.SaveAs(ms);
            return ms.ToArray();
        }

        public async Task CreateBankAccountWorksheet(IXLWorkbook workbook, BankDto bank)
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

            worksheet.Cell("A1").Value = bank.Name;
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
                worksheet.Cell($"B{currentRow}").Value = account.Balance;
                worksheet.Cell($"C{currentRow}").Value = account.Currency.Rate;

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
                worksheet.Cell($"B{currentRow}").Value = deposit.InitialAmount;
                worksheet.Cell($"C{currentRow}").Value = deposit.Currency.Rate;
                worksheet.Cell($"D{currentRow}").Value = deposit.Percentage;
                worksheet.Cell($"E{currentRow}").Value = deposit.From.ToDateTime(new TimeOnly());
                worksheet.Cell($"F{currentRow}").Value = totalDays;
                worksheet.Cell($"G{currentRow}").Value = deposit.EstimatedEarn / totalDays * daysPassed;

                total += income;
                totalDynamic += deposit.InitialAmount;
                notConfirmedIncomes += income;
                incomeInMonth += deposit.InitialAmount / 12 / 100 * deposit.Percentage;

                currentRow++;
            }

            worksheet.Cell($"A{reservedRowsBeforeSummaries}").Value = "Итого";
            worksheet.Cell($"B{reservedRowsBeforeSummaries++}").Value = total;

            worksheet.Cell($"A{reservedRowsBeforeSummaries}").Value = "Итого динамических";
            worksheet.Cell($"B{reservedRowsBeforeSummaries++}").Value = totalDynamic;

            worksheet.Cell($"A{reservedRowsBeforeSummaries}").Value = "В месяц";
            worksheet.Cell($"B{reservedRowsBeforeSummaries++}").Value = incomeInMonth;

            worksheet.Cell($"A{reservedRowsBeforeSummaries}").Value = "Только после завершения";
            worksheet.Cell($"B{reservedRowsBeforeSummaries++}").Value = notConfirmedIncomes;

            worksheet.Cell($"A{reservedRowsBeforeSummaries}").Value = "Итого статических";
            worksheet.Cell($"B{reservedRowsBeforeSummaries}").Value = totalStatic;

            worksheet.Columns().AdjustToContents();
        }

        public async Task CreateBrokerAccountWorksheet(IXLWorksheet worksheet)
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
                worksheet.Cell($"B{currentRow}").Value = amount;
                worksheet.Cell($"C{currentRow}").Value = currency.Rate;
                worksheet.Cell($"D{currentRow}").Value = currency.Rate * amount;

                currentRow++;
            }

            var brokerAccountSecurities = await _brokerAccountSecurityService.GetAll(true);

            foreach (var brokerAccountSecurity in brokerAccountSecurities)
            {
                worksheet.Cell($"A{currentRow}").Value = brokerAccountSecurity.Security.Ticker;
                worksheet.Cell($"B{currentRow}").Value = brokerAccountSecurity.Quantity;
                worksheet.Cell($"C{currentRow}").Value = brokerAccountSecurity.Security.ActualPrice;
                worksheet.Cell($"D{currentRow}").Value = brokerAccountSecurity.Security.ActualPrice * brokerAccountSecurity.Quantity;

                currentRow++;
            }
        }

        public async Task CreateDebtorsWorksheet(IXLWorksheet worksheet)
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
                worksheet.Cell($"B{currentRow}").Value = debt.Amount;
                worksheet.Cell($"C{currentRow}").Value = debt.Currency.Rate;

                total += debt.Currency.Rate * debt.Amount;

                currentRow++;
            }

            int totalsRow = 20;
            worksheet.Cell($"A{totalsRow}").Value = "Итого:";
            worksheet.Cell($"B{totalsRow}").Value = total;
        }
    }
}
