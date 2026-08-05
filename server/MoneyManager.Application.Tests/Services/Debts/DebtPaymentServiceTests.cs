using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.DTO.Banks;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Application.Interfaces.Banks;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Constants;

namespace MoneyManager.Application.Tests.Services.Debts
{
    //TODO: add update variations tests
    public class DebtPaymentServiceTests : TestBase
    {
        public DebtPaymentServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestGetById()
        {
            var (debtId, accountId) = await SetupDependencies(1000m, 500m);

            var payment = new DebtPaymentDto
            {
                DebtId = debtId,
                TargetAccountId = accountId,
                Amount = 200m,
                IsPercentagePayment = false,
                Date = DateOnly.FromDateTime(DateTime.Now)
            };

            var paymentId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtPaymentService>();
                return await service.Add(payment);
            });

            Assert.NotEqual(Guid.Empty, paymentId);

            var paymentFromDb = await ExecuteScopeAsync(async sp =>
            {
                var debtService = sp.GetRequiredService<IDebtPaymentService>();

                return await debtService.GetById(paymentId);
            });

            Assert.NotNull(paymentFromDb);
            Assert.Equal(payment.Amount, paymentFromDb.Amount);
            Assert.Equal(payment.TargetAccountId, paymentFromDb.TargetAccountId);
            Assert.Equal(payment.IsPercentagePayment, paymentFromDb.IsPercentagePayment);
            Assert.Equal(payment.Date, paymentFromDb.Date);
        }

        [Fact]
        public async Task TestAdd_UpdatesDebtAndAccountBalances()
        {
            var (debtId, accountId) = await SetupDependencies(1000m, 500m);

            var paymentId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtPaymentService>();
                return await service.Add(new DebtPaymentDto
                {
                    DebtId = debtId,
                    TargetAccountId = accountId,
                    Amount = 200m,
                    IsPercentagePayment = false,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                });
            });

            Assert.NotEqual(Guid.Empty, paymentId);

            var (debtAmount, accountBalance) = await ExecuteScopeAsync(async sp =>
            {
                var debtService = sp.GetRequiredService<IDebtService>();
                var accService = sp.GetRequiredService<IAccountService>();


                var debt = await debtService.GetById(debtId);
                var acc = await accService.GetById(accountId);

                return (debt.Amount, acc.Balance);
            });

            Assert.Equal(800m, debtAmount);       // 1000 - 200
            Assert.Equal(700m, accountBalance);   // 500 + 200
        }

        [Fact]
        public async Task TestGetPaginationAndGetAll()
        {
            var (debtId, accountId) = await SetupDependencies(1000m, 500m);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtPaymentService>();
                await service.Add(new DebtPaymentDto
                {
                    DebtId = debtId,
                    TargetAccountId = accountId,
                    Amount = 100m,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                });
            });

            var pagination = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtPaymentService>();
                return await service.GetPagination();
            });

            Assert.NotNull(pagination);
            Assert.True(pagination.RecordsQuantity >= 1);

            var payments = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtPaymentService>();
                return await service.GetAll(1, pagination.PageSize);
            });

            Assert.NotNull(payments);
            Assert.NotEmpty(payments);
        }

        [Fact]
        public async Task TestDelete_RevertsDebtAndAccount()
        {
            var (debtId, accountId) = await SetupDependencies(1000m, 500m);

            var paymentId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtPaymentService>();
                return await service.Add(new DebtPaymentDto
                {
                    DebtId = debtId,
                    TargetAccountId = accountId,
                    Amount = 300m,
                    IsPercentagePayment = false,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                });
            });

            var (debtAmountAfterPayment, accountBalanceAfterPayment) = await ExecuteScopeAsync(async sp =>
            {
                var debtService = sp.GetRequiredService<IDebtService>();
                var accService = sp.GetRequiredService<IAccountService>();

                var debt = await debtService.GetById(debtId);
                var acc = await accService.GetById(accountId);

                return (debt.Amount, acc.Balance);
            });

            Assert.Equal(700m, debtAmountAfterPayment);     // 1000 - 300
            Assert.Equal(800m, accountBalanceAfterPayment);   // 500 + 300

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtPaymentService>();
                await service.Delete(paymentId);
            });

            var (debtAmountAfterDelete, accountBalanceAfterDelete) = await ExecuteScopeAsync(async sp =>
            {
                var debtService = sp.GetRequiredService<IDebtService>();
                var accService = sp.GetRequiredService<IAccountService>();

                var debts = await debtService.GetAll(false);
                var debt = debts.First(d => d.Id == debtId);
                var acc = await accService.GetById(accountId);

                return (debt.Amount, acc.Balance);
            });

            Assert.Equal(1000m, debtAmountAfterDelete);     // Reverted to 1000
            Assert.Equal(500m, accountBalanceAfterDelete);   // Reverted to 500
        }

        [Fact]
        public async Task TestUpdate_AccountChanged()
        {
            var (debtId, initialAccountId) = await SetupDependencies(1000m, 500m);

            var newAccountId = await CreateAccount(1250m);

            var paymentId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtPaymentService>();
                return await service.Add(new DebtPaymentDto
                {
                    DebtId = debtId,
                    TargetAccountId = initialAccountId,
                    Amount = 300m,
                    IsPercentagePayment = false,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtPaymentService>();
                await service.Update(new DebtPaymentDto
                {
                    Id = paymentId,
                    DebtId = debtId,
                    TargetAccountId = newAccountId,
                    Amount = 300m,
                    IsPercentagePayment = false,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                });
            });


            await ExecuteScopeAsync(async sp =>
            {
                var accService = sp.GetRequiredService<IAccountService>();

                var initialAccount = await accService.GetById(initialAccountId);
                var newAccount = await accService.GetById(newAccountId);

                Assert.Equal(500m, initialAccount.Balance);     // Reverted to 1000
                Assert.Equal(1550m, newAccount.Balance);     // Increased by 1000
            });
        }

        [Fact]
        public async Task TestUpdate_AccountAmountChanged()
        {
            var initialPaymentAmount = 300m;
            var newPaymentAmount = 1000m;

            var (debtId, initialAccountId) = await SetupDependencies(1000m, 500m);

            var newAccountId = await CreateAccount(1250m);

            var paymentId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtPaymentService>();
                return await service.Add(new DebtPaymentDto
                {
                    DebtId = debtId,
                    TargetAccountId = initialAccountId,
                    Amount = initialPaymentAmount,
                    IsPercentagePayment = false,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtPaymentService>();
                await service.Update(new DebtPaymentDto
                {
                    Id = paymentId,
                    DebtId = debtId,
                    TargetAccountId = newAccountId,
                    Amount = newPaymentAmount,
                    IsPercentagePayment = false,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                });
            });


            await ExecuteScopeAsync(async sp =>
            {
                var accService = sp.GetRequiredService<IAccountService>();

                var initialAccount = await accService.GetById(initialAccountId);
                var newAccount = await accService.GetById(newAccountId);

                Assert.Equal(500m, initialAccount.Balance);     // Reverted to 500m
                Assert.Equal(2250m, newAccount.Balance);     // Increased by 1000
            });
        }

        [Fact]
        public async Task TestAdd_PercentagePayment()
        {
            var (debtId, accountId) = await SetupDependencies(1000m, 500m);

            var nonPercentageAmount = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtPaymentService>();
                var amount = 300m;
                await service.Add(new DebtPaymentDto
                {
                    DebtId = debtId,
                    TargetAccountId = accountId,
                    Amount = amount,
                    IsPercentagePayment = false,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                });

                return amount;
            });

            await ExecuteScopeAsync(async sp =>
            {
                var accService = sp.GetRequiredService<IAccountService>();
                var debtService = sp.GetRequiredService<IDebtService>();

                var account = await accService.GetById(accountId);
                var debt = await debtService.GetById(debtId);

                // both should be changed
                Assert.Equal(500m + nonPercentageAmount, account.Balance); 
                Assert.Equal(1000m - nonPercentageAmount, debt.Amount);
            });

            var percentageAmount = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtPaymentService>();
                var amount = 300m;
                await service.Add(new DebtPaymentDto
                {
                    DebtId = debtId,
                    TargetAccountId = accountId,
                    Amount = amount,
                    IsPercentagePayment = true,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                });

                return amount;
            });

            await ExecuteScopeAsync(async sp =>
            {
                var accService = sp.GetRequiredService<IAccountService>();
                var debtService = sp.GetRequiredService<IDebtService>();

                var account = await accService.GetById(accountId);
                var debt = await debtService.GetById(debtId);

                // should be changed
                Assert.Equal(500m + nonPercentageAmount + percentageAmount, account.Balance);
                // should be the same
                Assert.Equal(1000m - nonPercentageAmount, debt.Amount);
            });
        }

        private async Task<Guid> CreateAccount(decimal initialBalance)
        {
            var bank = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBankService>();
                return await service.Add(new BankDto()
                {
                    Id = Guid.NewGuid(),
                    Name = $"Test bank_{DateTime.UtcNow}"
                }, null);
            });

            var accountId = await ExecuteScopeAsync(async sp =>
            {
                var accService = sp.GetRequiredService<IAccountService>();
                return await accService.Add(new AccountDTO
                {
                    Active = true,
                    Name = $"Payment Test Acc_{DateTime.UtcNow}",
                    AccountTypeId = AccountTypeConstants.Cash,
                    CurrencyId = CurrencyConstants.USD,
                    Balance = initialBalance,
                    CreatedOn = DateOnly.FromDateTime(DateTime.Now),
                    BankId = bank.Id
                });
            });

            return accountId;
        }

        private async Task<(Guid debtId, Guid accountId)> SetupDependencies(decimal initialDebt, decimal initialBalance)
        {
            var debtId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtService>();
                return await service.Add(new DebtDto
                {
                    Name = "Payment Test Debt",
                    Amount = initialDebt,
                    CurrencyId = CurrencyConstants.USD,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                });
            });

            return (debtId, await CreateAccount(initialBalance));
        }
    }
}
