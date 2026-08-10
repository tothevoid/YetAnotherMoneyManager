using System.Collections.Generic;
using Riok.Mapperly.Abstractions;
using MoneyManager.Application.DTO;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.DTO.Banks;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Application.DTO.Crypto;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.Application.DTO.Deposits;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Banks;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Crypto;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Infrastructure.Entities.Debts;
using MoneyManager.Infrastructure.Entities.Deposits;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Entities.Transactions;
using MoneyManager.Infrastructure.Entities.User;

namespace MoneyManager.Application.Mappings
{
    [Mapper]
    public partial class ApplicationMapper
    {
        public partial Transaction Map(TransactionDTO dto);
        public partial TransactionDTO Map(Transaction entity);
        public partial IEnumerable<TransactionDTO> Map(IEnumerable<Transaction> entities);

        public partial Account Map(AccountDTO dto);
        public partial AccountDTO Map(Account entity);
        public partial IEnumerable<AccountDTO> Map(IEnumerable<Account> entities);

        public partial TransactionType Map(TransactionTypeDTO dto);
        public partial TransactionTypeDTO Map(TransactionType entity);
        public partial IEnumerable<TransactionTypeDTO> Map(IEnumerable<TransactionType> entities);

        public partial Deposit Map(DepositDTO dto);
        public partial DepositDTO Map(Deposit entity);
        public partial IEnumerable<DepositDTO> Map(IEnumerable<Deposit> entities);

        public partial Currency Map(CurrencyDTO dto);
        public partial CurrencyDTO Map(Currency entity);
        public partial IEnumerable<CurrencyDTO> Map(IEnumerable<Currency> entities);

        public partial AccountType Map(AccountTypeDTO dto);
        public partial AccountTypeDTO Map(AccountType entity);
        public partial IEnumerable<AccountTypeDTO> Map(IEnumerable<AccountType> entities);

        public partial CurrencyTransaction Map(CurrencyTransactionDto dto);
        public partial CurrencyTransactionDto Map(CurrencyTransaction entity);
        public partial IEnumerable<CurrencyTransactionDto> Map(IEnumerable<CurrencyTransaction> entities);

        public partial Security Map(SecurityDTO dto);
        public partial SecurityDTO Map(Security entity);
        public partial IEnumerable<SecurityDTO> Map(IEnumerable<Security> entities);

        public partial SecurityType Map(SecurityTypeDTO dto);
        public partial SecurityTypeDTO Map(SecurityType entity);
        public partial IEnumerable<SecurityTypeDTO> Map(IEnumerable<SecurityType> entities);

        public partial Dividend Map(DividendDto dto);
        public partial DividendDto Map(Dividend entity);
        public partial IEnumerable<DividendDto> Map(IEnumerable<Dividend> entities);

        public partial Broker Map(BrokerDTO dto);
        public partial BrokerDTO Map(Broker entity);
        public partial IEnumerable<BrokerDTO> Map(IEnumerable<Broker> entities);

        public partial BrokerAccount Map(BrokerAccountDTO dto);
        public partial BrokerAccountDTO Map(BrokerAccount entity);
        public partial IEnumerable<BrokerAccountDTO> Map(IEnumerable<BrokerAccount> entities);

        public partial BrokerAccountSecurity Map(BrokerAccountSecurityDTO dto);
        public partial BrokerAccountSecurityDTO Map(BrokerAccountSecurity entity);
        public partial IEnumerable<BrokerAccountSecurityDTO> Map(IEnumerable<BrokerAccountSecurity> entities);

        public partial BrokerAccountType Map(BrokerAccountTypeDTO dto);
        public partial BrokerAccountTypeDTO Map(BrokerAccountType entity);
        public partial IEnumerable<BrokerAccountTypeDTO> Map(IEnumerable<BrokerAccountType> entities);

        public partial SecurityTransaction Map(SecurityTransactionDTO dto);
        public partial SecurityTransactionDTO Map(SecurityTransaction entity);
        public partial IEnumerable<SecurityTransactionDTO> Map(IEnumerable<SecurityTransaction> entities);

        public partial UserProfile Map(UserProfileDto dto);
        public partial UserProfileDto Map(UserProfile entity);

        public partial Debt Map(DebtDto dto);
        public partial DebtDto Map(Debt entity);
        public partial IEnumerable<DebtDto> Map(IEnumerable<Debt> entities);

        public partial DebtPayment Map(DebtPaymentDto dto);
        public partial DebtPaymentDto Map(DebtPayment entity);
        public partial IEnumerable<DebtPaymentDto> Map(IEnumerable<DebtPayment> entities);

        public partial DividendPayment Map(DividendPaymentDto dto);
        public partial DividendPaymentDto Map(DividendPayment entity);
        public partial IEnumerable<DividendPaymentDto> Map(IEnumerable<DividendPayment> entities);

        public partial CryptoAccountCryptocurrency Map(CryptoAccountCryptocurrencyDto dto);
        public partial CryptoAccountCryptocurrencyDto Map(CryptoAccountCryptocurrency entity);
        public partial IEnumerable<CryptoAccountCryptocurrencyDto> Map(IEnumerable<CryptoAccountCryptocurrency> entities);

        public partial CryptoAccount Map(CryptoAccountDto dto);
        public partial CryptoAccountDto Map(CryptoAccount entity);
        public partial IEnumerable<CryptoAccountDto> Map(IEnumerable<CryptoAccount> entities);

        public partial Cryptocurrency Map(CryptocurrencyDto dto);
        public partial CryptocurrencyDto Map(Cryptocurrency entity);
        public partial IEnumerable<CryptocurrencyDto> Map(IEnumerable<Cryptocurrency> entities);

        public partial CryptoProvider Map(CryptoProviderDto dto);
        public partial CryptoProviderDto Map(CryptoProvider entity);
        public partial IEnumerable<CryptoProviderDto> Map(IEnumerable<CryptoProvider> entities);

        public partial BrokerAccountFundsTransfer Map(BrokerAccountFundsTransferDto dto);
        public partial BrokerAccountFundsTransferDto Map(BrokerAccountFundsTransfer entity);
        public partial IEnumerable<BrokerAccountFundsTransferDto> Map(IEnumerable<BrokerAccountFundsTransfer> entities);

        public partial Bank Map(BankDto dto);
        public partial BankDto Map(Bank entity);
        public partial IEnumerable<BankDto> Map(IEnumerable<Bank> entities);

        public partial BrokerAccountTaxDeduction Map(BrokerAccountTaxDeductionDto dto);
        public partial BrokerAccountTaxDeductionDto Map(BrokerAccountTaxDeduction entity);
        public partial IEnumerable<BrokerAccountTaxDeductionDto> Map(IEnumerable<BrokerAccountTaxDeduction> entities);
    }
}
