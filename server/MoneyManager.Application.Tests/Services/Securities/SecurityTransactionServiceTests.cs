using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Constants;

namespace MoneyManager.Application.Tests.Services.Securities
{
    public class SecurityTransactionServiceTests : TestBase
    {
        public SecurityTransactionServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetAll()
        {
            var (securityId, brokerAccountId) = await SetupDependencies();

            var dto = new SecurityTransactionDTO
            {
                SecurityId = securityId,
                BrokerAccountId = brokerAccountId,
                Quantity = 10,
                Price = 150m,
                Date = DateTime.UtcNow,
                BrokerCommission = 1.5m,
                StockExchangeCommission = 0.5m,
                IsSell = false
            };

            var addedId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                return await service.Add(dto);
            });

            Assert.NotEqual(Guid.Empty, addedId);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                return await service.GetAll(brokerAccountId, 10, 1);
            });

            Assert.NotNull(all);
            Assert.Contains(all, st => st.Id == addedId && st.Quantity == 10);
        }

        [Fact]
        public async Task TestUpdateAndDelete()
        {
            var (securityId, brokerAccountId) = await SetupDependencies();

            var dto = new SecurityTransactionDTO
            {
                SecurityId = securityId,
                BrokerAccountId = brokerAccountId,
                Quantity = 5,
                Price = 200m,
                Date = DateTime.UtcNow,
                IsSell = false
            };

            var addedId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                return await service.Add(dto);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                await service.Update(new SecurityTransactionDTO
                {
                    Id = addedId,
                    SecurityId = securityId,
                    BrokerAccountId = brokerAccountId,
                    Quantity = 20,
                    Price = 210m,
                    Date = DateTime.UtcNow,
                    IsSell = true
                });
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                return await service.GetAll(brokerAccountId, 10, 1);
            });

            var updated = all.FirstOrDefault(st => st.Id == addedId);
            Assert.NotNull(updated);
            Assert.Equal(20, updated.Quantity);
            Assert.True(updated.IsSell);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                await service.Delete(addedId);
            });

            var listAfterDelete = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                return await service.GetAll(brokerAccountId, 10, 1);
            });

            Assert.DoesNotContain(listAfterDelete, st => st.Id == addedId);
        }

        [Fact]
        public async Task TestGetSummaryTillSpecificDate()
        {
            var (securityId, broker1AccountId) = await SetupDependencies();
            var (_, broker2AccountId) = await SetupDependencies();
            var targetDate = new DateOnly(2020, 4, 1);

            // Broker 1 transactions
            // Buy 10 @ 100, Tax 2, Broker 1, Exchange 1 => PurchasePriceSum = 1000 + 2 + 1 + 1 = 1004
            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                await service.Add(new SecurityTransactionDTO
                {
                    SecurityId = securityId,
                    BrokerAccountId = broker1AccountId,
                    Quantity = 10,
                    Price = 100m,
                    Tax = 2m,
                    BrokerCommission = 1m,
                    StockExchangeCommission = 1m,
                    IsSell = false,
                    Date = new DateTime(2020, 3, 10, 10, 0, 0, DateTimeKind.Utc)
                });
            });

            // Sell 4 @ 120, Tax 1, Broker 1, Exchange 1 => SellPriceSum = 480 - 1 - 1 - 1 = 477
            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                await service.Add(new SecurityTransactionDTO
                {
                    SecurityId = securityId,
                    BrokerAccountId = broker1AccountId,
                    Quantity = 4,
                    Price = 120m,
                    Tax = 1m,
                    BrokerCommission = 1m,
                    StockExchangeCommission = 1m,
                    IsSell = true,
                    Date = new DateTime(2020, 3, 20, 10, 0, 0, DateTimeKind.Utc)
                });
            });

            // Buy after target date (should be ignored)
            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                await service.Add(new SecurityTransactionDTO
                {
                    SecurityId = securityId,
                    BrokerAccountId = broker1AccountId,
                    Quantity = 5,
                    Price = 150m,
                    IsSell = false,
                    Date = new DateTime(2020, 4, 10, 10, 0, 0, DateTimeKind.Utc)
                });
            });

            // Broker 2 transactions
            // Buy 6 @ 200, Tax 0, Broker 0, Exchange 0 => PurchasePriceSum = 1200
            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                await service.Add(new SecurityTransactionDTO
                {
                    SecurityId = securityId,
                    BrokerAccountId = broker2AccountId,
                    Quantity = 6,
                    Price = 200m,
                    IsSell = false,
                    Date = new DateTime(2020, 3, 15, 10, 0, 0, DateTimeKind.Utc)
                });
            });

            // Sell after target date (should be ignored)
            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                await service.Add(new SecurityTransactionDTO
                {
                    SecurityId = securityId,
                    BrokerAccountId = broker2AccountId,
                    Quantity = 2,
                    Price = 210m,
                    IsSell = true,
                    Date = new DateTime(2020, 4, 15, 10, 0, 0, DateTimeKind.Utc)
                });
            });

            // Verify Broker 1 Summary
            var summaryB1 = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                return await service.GetSummaryTillSpecificDate(targetDate, broker1AccountId);
            });

            Assert.NotNull(summaryB1);
            Assert.True(summaryB1.ContainsKey("MSFT"));
            var msftB1 = summaryB1["MSFT"];
            Assert.Equal(6, msftB1.ActualQuantity); // 10 bought - 4 sold
            Assert.Equal(1004m, msftB1.PurchasePriceSum);
            Assert.Equal(477m, msftB1.SellPriceSum);

            // Verify Broker 2 Summary
            var summaryB2 = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                return await service.GetSummaryTillSpecificDate(targetDate, broker2AccountId);
            });

            Assert.NotNull(summaryB2);
            Assert.True(summaryB2.ContainsKey("MSFT"));
            var msftB2 = summaryB2["MSFT"];
            Assert.Equal(6, msftB2.ActualQuantity);
            Assert.Equal(1200m, msftB2.PurchasePriceSum);
            Assert.Equal(0m, msftB2.SellPriceSum);

            // Verify All Brokers Summary (brokerAccountId = null)
            var summaryAll = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                return await service.GetSummaryTillSpecificDate(targetDate, null);
            });

            Assert.NotNull(summaryAll);
            Assert.True(summaryAll.ContainsKey("MSFT"));
            var msftAll = summaryAll["MSFT"];
            Assert.Equal(12, msftAll.ActualQuantity); // 6 + 6
            Assert.Equal(2204m, msftAll.PurchasePriceSum); // 1004 + 1200
            Assert.Equal(477m, msftAll.SellPriceSum);
        }

        private async Task<(Guid securityId, Guid brokerAccountId)> SetupDependencies()
        {
            var securityTypeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTypeService>();
                return await service.Add(new SecurityTypeDTO { Name = "SecTx Stock" });
            });

            var securityId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                var sec = await service.Add(new SecurityDTO
                {
                    Name = "Microsoft",
                    Ticker = "MSFT",
                    TypeId = securityTypeId,
                    CurrencyId = CurrencyConstants.USD,
                    ActualPrice = 400m
                }, null);
                return sec.Id;
            });

            var brokerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                return await service.Add(new BrokerDTO { Name = "SecTx Broker" });
            });

            var brokerAccountTypeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                return await service.Add(new BrokerAccountTypeDTO { Name = "Standard Broker Acc" });
            });

            var brokerAccountId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountService>();
                return await service.Add(new BrokerAccountDTO
                {
                    Name = "SecTx Broker Acc",
                    BrokerId = brokerId,
                    TypeId = brokerAccountTypeId,
                    CurrencyId = CurrencyConstants.USD
                });
            });

            return (securityId, brokerAccountId);
        }
    }
}
