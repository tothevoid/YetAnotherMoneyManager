using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Integrations.Currency;
using MoneyManager.Application.Integrations.Stock.Moex;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Application.Interfaces.Auth;
using MoneyManager.Application.Interfaces.Banks;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Application.Interfaces.Currencies;
using MoneyManager.Application.Interfaces.Dashboard;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.Application.Interfaces.Deposits;
using MoneyManager.Application.Interfaces.FileStorage;
using MoneyManager.Application.Interfaces.Integrations.Currency;
using MoneyManager.Application.Interfaces.Integrations.Stock;
using MoneyManager.Application.Interfaces.Notifications;
using MoneyManager.Application.Interfaces.Reports;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.Application.Mappings;
using MoneyManager.Application.Services.Accounts;
using MoneyManager.Application.Services.Auth;
using MoneyManager.Application.Services.Banks;
using MoneyManager.Application.Services.Brokers;
using MoneyManager.Application.Services.Crypto;
using MoneyManager.Application.Services.Currencies;
using MoneyManager.Application.Services.Dashboard;
using MoneyManager.Application.Services.Debts;
using MoneyManager.Application.Services.Deposits;
using MoneyManager.Application.Services.FileStorage;
using MoneyManager.Application.Services.Notifications;
using MoneyManager.Application.Services.Reports;
using MoneyManager.Application.Services.Securities;
using MoneyManager.Application.Services.Transactions;
using MoneyManager.Application.Services.User;

namespace MoneyManager.Application.Extensions
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddSingleton<ApplicationMapper>();
            services.AddTransient<ITransactionsService, TransactionsService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IDepositService, DepositService>();
            services.AddTransient<ITransactionTypeService, TransactionTypeService>();
            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<IAccountTypeService, AccountTypeService>();
            services.AddTransient<IBrokerAccountSecurityService, BrokerAccountSecurityService>();
            services.AddTransient<IBrokerAccountService, BrokerAccountService>();
            services.AddTransient<IBrokerAccountTypeService, BrokerAccountTypeService>();
            services.AddTransient<IBrokerService, BrokerService>();
            services.AddTransient<IBrokerAccountSummaryService, BrokerAccountSummaryService>();
            services.AddTransient<ISecurityService, SecurityService>();
            services.AddTransient<ISecurityTransactionService, SecurityTransactionService>();
            services.AddTransient<ISecurityTypeService, SecurityTypeService>();
            services.AddTransient<IDividendService, DividendService>();
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IDebtService, DebtService>();
            services.AddTransient<IDebtTagService, DebtTagService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IDebtPaymentService, DebtPaymentService>();
            services.AddTransient<IDividendPaymentService, DividendPaymentService>();
            services.AddTransient<ICurrencyTransactionService, CurrencyTransactionService>();
            services.AddTransient<IBrokerAccountFundsTransferService, BrokerAccountFundsTransferService>();
            services.AddTransient<IBrokerAccountTaxDeductionService, BrokerAccountTaxDeductionService>();
            services.AddTransient<ICryptoAccountService, CryptoAccountService>();
            services.AddTransient<ICryptoAccountCryptocurrencyService, CryptoAccountCryptocurrencyService>();
            services.AddTransient<ICryptocurrencyService, CryptocurrencyService>();
            services.AddTransient<ICryptoProviderService, CryptoProviderService>();
            services.AddTransient<IBankService, BankService>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddTransient<IAllAssetsReportService, AllAssetsReportService>();
            services.AddTransient<IBrokerAccountPortfolioHistoryService, BrokerAccountPortfolioHistoryService>();
            services.AddSingleton<IPullQuotationsService, PullQuotationsService>();

            //TODO: make factory
            //TODO: possible change AddTransient to AddSingleton
            services.AddTransient<IStockConnector, MoexConnector>();
            services.AddTransient<ICurrencyGrabber, CbrCurrencyGrabber>();

            services.AddScoped<IFileStorageService, FileStorageService>();

            return services;
        }
    }
}
