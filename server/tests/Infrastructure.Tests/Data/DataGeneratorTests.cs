using MoneyManager.Infrastructure.Data;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Entities.Transactions;
using MoneyManager.Infrastructure.Entities.User;
using Xunit;

namespace MoneyManager.Infrastructure.Tests.Data
{
    public class DataGeneratorTests
    {
        [Fact]
        public void AccountTypeGenerator_ReturnsDefaultAccountTypes()
        {
            var generator = new AccountTypeGenerator();

            AccountType[] types = generator.Generate();

            Assert.NotNull(types);
            Assert.NotEmpty(types);
            Assert.All(types, item =>
            {
                Assert.NotEqual(Guid.Empty, item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.Name));
                Assert.True(item.Active);
            });
        }

        [Fact]
        public void CurrencyGenerator_ReturnsDefaultCurrencies()
        {
            var generator = new CurrencyGenerator();

            Currency[] currencies = generator.Generate();

            Assert.NotNull(currencies);
            Assert.NotEmpty(currencies);
            Assert.All(currencies, item =>
            {
                Assert.NotEqual(Guid.Empty, item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.Name));
            });
        }

        [Fact]
        public void SecurityTypeGenerator_ReturnsDefaultSecurityTypes()
        {
            var generator = new SecurityTypeGenerator();

            SecurityType[] types = generator.Generate();

            Assert.NotNull(types);
            Assert.NotEmpty(types);
            Assert.All(types, item =>
            {
                Assert.NotEqual(Guid.Empty, item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.Name));
            });
        }

        [Fact]
        public void TransactionTypeGenerator_ReturnsDefaultTransactionTypes()
        {
            var generator = new TransactionTypeGenerator();

            TransactionType[] types = generator.Generate();

            Assert.NotNull(types);
            Assert.NotEmpty(types);
            Assert.All(types, item =>
            {
                Assert.NotEqual(Guid.Empty, item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.Name));
            });
        }

        [Fact]
        public void UserProfileGenerator_ReturnsDefaultUserProfile()
        {
            var generator = new UserProfileGenerator();

            UserProfile[] profiles = generator.Generate();

            Assert.NotNull(profiles);
            Assert.Single(profiles);
            Assert.NotEqual(Guid.Empty, profiles[0].Id);
        }
    }
}
