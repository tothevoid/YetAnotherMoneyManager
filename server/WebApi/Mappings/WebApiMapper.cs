using System.Collections.Generic;
using Riok.Mapperly.Abstractions;
using MoneyManager.Application.DTO;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.DTO.Banks;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Common;
using MoneyManager.Application.DTO.Crypto;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Application.DTO.Dashboard;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.Application.DTO.Deposits;
using MoneyManager.Application.DTO.Notifications;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.WebApi.Models.Accounts;
using MoneyManager.WebApi.Models.Auth;
using MoneyManager.WebApi.Models.Banks;
using MoneyManager.WebApi.Models.Brokers;
using MoneyManager.WebApi.Models.Common;
using MoneyManager.WebApi.Models.Crypto;
using MoneyManager.WebApi.Models.Currencies;
using MoneyManager.WebApi.Models.Dashboard;
using MoneyManager.WebApi.Models.Debts;
using MoneyManager.WebApi.Models.Deposits;
using MoneyManager.WebApi.Models.Deposits.Charts;
using MoneyManager.WebApi.Models.Notifications;
using MoneyManager.WebApi.Models.Securities;
using MoneyManager.WebApi.Models.Transactions;
using MoneyManager.WebApi.Models.User;

namespace MoneyManager.WebApi.Mappings
{
    // EF navigation properties will be ignored by the mapper so RequiredMappingStrategy = None is ok.
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class WebApiMapper
    {
        public partial TransactionDTO Map(TransactionModel model);
        public partial TransactionModel Map(TransactionDTO dto);
        public partial IEnumerable<TransactionModel> Map(IEnumerable<TransactionDTO> dtos);

        public partial AccountDTO Map(AccountModel model);
        public partial AccountModel Map(AccountDTO dto);
        public partial IEnumerable<AccountModel> Map(IEnumerable<AccountDTO> dtos);

        public partial TransactionTypeDTO Map(TransactionTypeModel model);
        public partial TransactionTypeModel Map(TransactionTypeDTO dto);
        public partial IEnumerable<TransactionTypeModel> Map(IEnumerable<TransactionTypeDTO> dtos);

        public partial DepositDTO Map(DepositModel model);
        public partial DepositModel Map(DepositDTO dto);
        public partial IEnumerable<DepositModel> Map(IEnumerable<DepositDTO> dtos);

        public partial CurrencyDTO Map(CurrencyModel model);
        public partial CurrencyModel Map(CurrencyDTO dto);
        public partial IEnumerable<CurrencyModel> Map(IEnumerable<CurrencyDTO> dtos);

        public partial AccountTypeDTO Map(AccountTypeModel model);
        public partial AccountTypeModel Map(AccountTypeDTO dto);
        public partial IEnumerable<AccountTypeModel> Map(IEnumerable<AccountTypeDTO> dtos);

        public partial CurrencyTransactionDto Map(CurrencyTransactionModel model);
        public partial CurrencyTransactionModel Map(CurrencyTransactionDto dto);
        public partial IEnumerable<CurrencyTransactionModel> Map(IEnumerable<CurrencyTransactionDto> dtos);

        public partial DepositsRangeModel Map(DepositsRangeDTO dto);
        public partial DepositMonthSummary Map(DepositMonthSummaryDTO dto);
        public partial DepositPayment Map(DepositPaymentDTO dto);
        public partial PeriodPayment Map(PeriodPaymentDTO dto);
        public partial AccountTransferDTO Map(AccountTransferModel model);
        public partial SecurityHistoryValueModel Map(SecurityHistoryValueDto dto);
        public partial SecurityHistoryValueDto Map(SecurityHistoryValueModel model);
        public partial IEnumerable<SecurityHistoryValueModel> Map(IEnumerable<SecurityHistoryValueDto> dtos);

        public partial AccountCurrencySummaryModel Map(AccountCurrencySummaryDTO dto);
        public partial IEnumerable<AccountCurrencySummaryModel> Map(IEnumerable<AccountCurrencySummaryDTO> dtos);

        public partial SecurityDTO Map(SecurityModel model);
        public partial SecurityModel Map(SecurityDTO dto);
        public partial IEnumerable<SecurityModel> Map(IEnumerable<SecurityDTO> dtos);

        public partial DividendDto Map(DividendModel model);
        public partial DividendModel Map(DividendDto dto);
        public partial IEnumerable<DividendModel> Map(IEnumerable<DividendDto> dtos);

        public partial SecurityTypeDTO Map(SecurityTypeModel model);
        public partial SecurityTypeModel Map(SecurityTypeDTO dto);
        public partial IEnumerable<SecurityTypeModel> Map(IEnumerable<SecurityTypeDTO> dtos);

        public partial BrokerDTO Map(BrokerModel model);
        public partial BrokerModel Map(BrokerDTO dto);
        public partial IEnumerable<BrokerModel> Map(IEnumerable<BrokerDTO> dtos);

        public partial BrokerAccountDTO Map(BrokerAccountModel model);
        public partial BrokerAccountModel Map(BrokerAccountDTO dto);
        public partial IEnumerable<BrokerAccountModel> Map(IEnumerable<BrokerAccountDTO> dtos);

        public partial BrokerAccountSecurityDTO Map(BrokerAccountSecurityModel model);
        public partial BrokerAccountSecurityModel Map(BrokerAccountSecurityDTO dto);
        public partial IEnumerable<BrokerAccountSecurityModel> Map(IEnumerable<BrokerAccountSecurityDTO> dtos);

        public partial BrokerAccountTypeDTO Map(BrokerAccountTypeModel model);
        public partial BrokerAccountTypeModel Map(BrokerAccountTypeDTO dto);
        public partial IEnumerable<BrokerAccountTypeModel> Map(IEnumerable<BrokerAccountTypeDTO> dtos);

        public partial SecurityTransactionDTO Map(SecurityTransactionModel model);
        public partial SecurityTransactionModel Map(SecurityTransactionDTO dto);
        public partial IEnumerable<SecurityTransactionModel> Map(IEnumerable<SecurityTransactionDTO> dtos);

        public partial UserProfileDto Map(UserProfileModel model);
        public partial UserProfileModel Map(UserProfileDto dto);

        public partial GlobalDashboardModel Map(GlobalDashboardDto dto);
        public partial TransactionsGlobalDashboardModel Map(TransactionsGlobalDashboardDto dto);
        public partial BrokerAccountsGlobalDashboardModel Map(BrokerAccountsGlobalDashboardDto dto);
        public partial AccountsGlobalDashboardModel Map(AccountsGlobalDashboardDto dto);

        public partial DistributionModel Map(DistributionDto dto);
        public partial DistributionDto Map(DistributionModel model);

        public partial DebtsGlobalDashboardModel Map(DebtsGlobalDashboardDto dto);
        public partial DebtsGlobalDashboardDto Map(DebtsGlobalDashboardModel model);

        public partial DepositsGlobalDashboardDto Map(DepositsGlobalDashboardModel model);
        public partial DepositsGlobalDashboardModel Map(DepositsGlobalDashboardDto dto);

        public partial CryptoAccountsGlobalDashboardModel Map(CryptoAccountsGlobalDashboardDto dto);
        public partial CryptoAccountsGlobalDashboardDto Map(CryptoAccountsGlobalDashboardModel model);

        public partial BanksGlobalDashboardModel Map(BanksGlobalDashboardDto dto);
        public partial BanksGlobalDashboardDto Map(BanksGlobalDashboardModel model);

        public partial SecurityTransactionsHistoryModel Map(SecurityTransactionsHistoryDto dto);
        public partial IEnumerable<SecurityTransactionsHistoryModel> Map(IEnumerable<SecurityTransactionsHistoryDto> dtos);

        public partial DebtModel Map(DebtDto dto);
        public partial DebtDto Map(DebtModel model);
        public partial IEnumerable<DebtModel> Map(IEnumerable<DebtDto> dtos);

        public partial DebtTagModel Map(DebtTagDto dto);
        public partial DebtTagDto Map(DebtTagModel model);
        public partial IEnumerable<DebtTagModel> Map(IEnumerable<DebtTagDto> dtos);

        public partial DebtTagStatsModel Map(DebtTagStatsDto dto);
        public partial IEnumerable<DebtTagStatsModel> Map(IEnumerable<DebtTagStatsDto> dtos);

        public partial DebtPaymentModel Map(DebtPaymentDto dto);
        public partial DebtPaymentDto Map(DebtPaymentModel model);
        public partial IEnumerable<DebtPaymentModel> Map(IEnumerable<DebtPaymentDto> dtos);

        public partial DividendPaymentModel Map(DividendPaymentDto dto);
        public partial DividendPaymentDto Map(DividendPaymentModel model);
        public partial IEnumerable<DividendPaymentModel> Map(IEnumerable<DividendPaymentDto> dtos);

        public partial SecurityStatsModel Map(SecurityStatsDto dto);
        public partial SecurityStatsDto Map(SecurityStatsModel model);

        public partial CryptoAccountCryptocurrencyModel Map(CryptoAccountCryptocurrencyDto dto);
        public partial CryptoAccountCryptocurrencyDto Map(CryptoAccountCryptocurrencyModel model);
        public partial IEnumerable<CryptoAccountCryptocurrencyModel> Map(IEnumerable<CryptoAccountCryptocurrencyDto> dtos);

        public partial CryptoAccountModel Map(CryptoAccountDto dto);
        public partial CryptoAccountDto Map(CryptoAccountModel model);
        public partial IEnumerable<CryptoAccountModel> Map(IEnumerable<CryptoAccountDto> dtos);

        public partial CryptocurrencyModel Map(CryptocurrencyDto dto);
        public partial CryptocurrencyDto Map(CryptocurrencyModel model);
        public partial IEnumerable<CryptocurrencyModel> Map(IEnumerable<CryptocurrencyDto> dtos);

        public partial CryptoProviderModel Map(CryptoProviderDto dto);
        public partial CryptoProviderDto Map(CryptoProviderModel model);
        public partial IEnumerable<CryptoProviderModel> Map(IEnumerable<CryptoProviderDto> dtos);

        public partial BrokerAccountFundsTransferModel Map(BrokerAccountFundsTransferDto dto);
        public partial BrokerAccountFundsTransferDto Map(BrokerAccountFundsTransferModel model);
        public partial IEnumerable<BrokerAccountFundsTransferModel> Map(IEnumerable<BrokerAccountFundsTransferDto> dtos);

        public partial BrokerAccountSummaryModel Map(BrokerAccountSummaryDto dto);
        public partial BrokerAccountSummaryDto Map(BrokerAccountSummaryModel model);

        public partial BrokerAccountStatsModel Map(BrokerAccountStatsDto dto);
        public partial BrokerAccountStatsDto Map(BrokerAccountStatsModel model);

        public partial BrokerAccountTransfersStatsModel Map(BrokerAccountTransfersStatsDto dto);
        public partial BrokerAccountTransfersStatsDto Map(BrokerAccountTransfersStatsModel model);

        public partial BrokerAccountMonthTransferModel Map(BrokerAccountMonthTransferDto dto);
        public partial BrokerAccountMonthTransferDto Map(BrokerAccountMonthTransferModel model);
        public partial IEnumerable<BrokerAccountMonthTransferModel> Map(IEnumerable<BrokerAccountMonthTransferDto> dtos);

        public partial BrokerAccountDayTransferModel Map(BrokerAccountDayTransferDto dto);
        public partial BrokerAccountDayTransferDto Map(BrokerAccountDayTransferModel model);
        public partial IEnumerable<BrokerAccountDayTransferModel> Map(IEnumerable<BrokerAccountDayTransferDto> dtos);

        public partial BrokerAccountDailyStatsModel Map(BrokerAccountDailyStatsDto dto);
        public partial BrokerAccountDailyStatsDto Map(BrokerAccountDailyStatsModel model);

        public partial BrokerAccountDailySecurityStatsModel Map(BrokerAccountDailySecurityStatsDto dto);
        public partial BrokerAccountDailySecurityStatsDto Map(BrokerAccountDailySecurityStatsModel model);

        public partial BankModel Map(BankDto dto);
        public partial BankDto Map(BankModel model);
        public partial IEnumerable<BankModel> Map(IEnumerable<BankDto> dtos);

        public partial PaginationConfigModel Map(PaginationConfigDto dto);

        public partial LoginModel Map(LoginDto dto);
        public partial LoginDto Map(LoginModel model);

        public partial BrokerAccountTaxDeductionDto Map(BrokerAccountTaxDeductionModel model);
        public partial BrokerAccountTaxDeductionModel Map(BrokerAccountTaxDeductionDto dto);
        public partial IEnumerable<BrokerAccountTaxDeductionModel> Map(IEnumerable<BrokerAccountTaxDeductionDto> dtos);

        public partial BrokerAccountPortfolioModel Map(BrokerAccountPortfolioDto dto);

        public partial NotificationDto Map(NotificationModel model);
        public partial NotificationModel Map(NotificationDto dto);
        public partial IEnumerable<NotificationModel> Map(IEnumerable<NotificationDto> dtos);
    }
}
