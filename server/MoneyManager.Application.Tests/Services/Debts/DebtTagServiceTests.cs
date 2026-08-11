using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Constants;
using Xunit;

namespace MoneyManager.Application.Tests.Services.Debts
{
    public class DebtTagServiceTests : TestBase
    {
        public DebtTagServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetAll()
        {
            var tagName = "Urgent";
            var colorHex = "#FF0000";

            var tagId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtTagService>();
                return await service.Add(new DebtTagDto
                {
                    Name = tagName,
                    ColorHex = colorHex
                });
            });

            Assert.NotEqual(Guid.Empty, tagId);

            var tags = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtTagService>();
                return await service.GetAll();
            });

            Assert.NotNull(tags);
            var created = tags.FirstOrDefault(t => t.Id == tagId);
            Assert.NotNull(created);
            Assert.Equal(tagName, created.Name);
            Assert.Equal(colorHex, created.ColorHex);
        }

        [Fact]
        public async Task TestUpdate()
        {
            var tagId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtTagService>();
                return await service.Add(new DebtTagDto
                {
                    Name = "Family",
                    ColorHex = "#00FF00"
                });
            });

            var updatedName = "Family & Friends";
            var updatedColor = "#0000FF";

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtTagService>();
                await service.Update(new DebtTagDto
                {
                    Id = tagId,
                    Name = updatedName,
                    ColorHex = updatedColor
                });
            });

            var fetched = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtTagService>();
                return await service.GetById(tagId);
            });

            Assert.NotNull(fetched);
            Assert.Equal(updatedName, fetched.Name);
            Assert.Equal(updatedColor, fetched.ColorHex);
        }

        [Fact]
        public async Task TestDelete()
        {
            var tagId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtTagService>();
                return await service.Add(new DebtTagDto
                {
                    Name = "Temporary",
                    ColorHex = "#CCCCCC"
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtTagService>();
                await service.Delete(tagId);
            });

            var tags = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtTagService>();
                return await service.GetAll();
            });

            Assert.DoesNotContain(tags, t => t.Id == tagId);
        }

        [Fact]
        public async Task TestGetStats()
        {
            var tagId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDebtTagService>();
                return await service.Add(new DebtTagDto
                {
                    Name = "Car Loan",
                    ColorHex = "#123456"
                });
            });

            var debtId = await ExecuteScopeAsync(async sp =>
            {
                var debtService = sp.GetRequiredService<IDebtService>();
                var tagService = sp.GetRequiredService<IDebtTagService>();
                var tag = await tagService.GetById(tagId);

                return await debtService.Add(new DebtDto
                {
                    Name = "Auto Parts",
                    Amount = 1000m,
                    CurrencyId = CurrencyConstants.USD,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    DebtTags = new System.Collections.Generic.List<DebtTagDto> { tag }
                });
            });

            var stats = await ExecuteScopeAsync(async sp =>
            {
                var tagService = sp.GetRequiredService<IDebtTagService>();
                return await tagService.GetStats();
            });

            var tagStats = stats.FirstOrDefault(s => s.TagId == tagId);
            Assert.NotNull(tagStats);
            Assert.Equal(1000m, tagStats.RemainingAmount);
            Assert.Equal(0m, tagStats.TotalPaid);
            Assert.Equal(1000m, tagStats.TotalAmount);
            Assert.Equal(0, tagStats.RepaymentPercentage);
        }
    }
}
