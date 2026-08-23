using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Application.Interfaces.DatabaseBackup;
using MoneyManager.Tests.Shared.Fixtures;
using Xunit;

namespace MoneyManager.Application.Tests.Services.DatabaseBackup
{
    public class DatabaseBackupServiceTests : TestBase
    {
        public DatabaseBackupServiceTests(ServiceProviderFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task CreateBackupAsync_Unencrypted_ShouldProduceValidBackup()
        {
            await ExecuteScopeAsync(async sp =>
            {
                var backupService = sp.GetRequiredService<IDatabaseBackupService>();

                var backupBytes = await backupService.CreateBackupAsync();

                Assert.NotNull(backupBytes);
                Assert.True(backupBytes.Length > 0);

                var validationResult = await backupService.ValidateBackupAsync(backupBytes);
                Assert.True(validationResult.IsValid);
                Assert.False(validationResult.IsEncrypted);
                Assert.Null(validationResult.ErrorMessage);
            });
        }

        [Fact]
        public async Task CreateBackupAsync_WithPassword_ShouldProduceEncryptedBackupAndValidate()
        {
            await ExecuteScopeAsync(async sp =>
            {
                var backupService = sp.GetRequiredService<IDatabaseBackupService>();
                var password = "BackupPassword123!@#";

                var encryptedBytes = await backupService.CreateBackupAsync(password);

                Assert.NotNull(encryptedBytes);
                Assert.True(encryptedBytes.Length > 0);

                // Validation without password should indicate encrypted
                var valWithoutPass = await backupService.ValidateBackupAsync(encryptedBytes);
                Assert.True(valWithoutPass.IsValid);
                Assert.True(valWithoutPass.IsEncrypted);

                // Validation with correct password should return valid metadata
                var valWithPass = await backupService.ValidateBackupAsync(encryptedBytes, password);
                Assert.True(valWithPass.IsValid);
                Assert.True(valWithPass.IsEncrypted);
                Assert.Null(valWithPass.ErrorMessage);

                // Validation with wrong password should fail
                var valWrongPass = await backupService.ValidateBackupAsync(encryptedBytes, "WrongPassword");
                Assert.False(valWrongPass.IsValid);
                Assert.NotNull(valWrongPass.ErrorMessage);
            });
        }

        [Fact]
        public async Task RestoreBackupAsync_Roundtrip_ShouldRestoreDataSuccessfully()
        {
            var testAccountName = $"BackupTestAccount_{Guid.NewGuid()}";
            var password = "RestoreTestPass456!";

            // Step 1: Create a test account type
            await ExecuteScopeAsync(async sp =>
            {
                var accountTypeService = sp.GetRequiredService<IAccountTypeService>();
                await accountTypeService.AddAsync(new AccountTypeDto
                {
                    Id = Guid.NewGuid(),
                    Name = testAccountName,
                    Active = true
                });
            });

            byte[] backupData = null!;

            // Step 2: Create encrypted backup
            await ExecuteScopeAsync(async sp =>
            {
                var backupService = sp.GetRequiredService<IDatabaseBackupService>();
                backupData = await backupService.CreateBackupAsync(password);
            });

            Assert.NotNull(backupData);

            // Step 3: Restore from backup
            await ExecuteScopeAsync(async sp =>
            {
                var backupService = sp.GetRequiredService<IDatabaseBackupService>();
                var restoreResult = await backupService.RestoreBackupAsync(backupData, password);

                Assert.True(restoreResult.Success);
                Assert.NotNull(restoreResult.Message);
            });

            // Step 4: Verify test account type exists after restore
            await ExecuteScopeAsync(async sp =>
            {
                var accountTypeService = sp.GetRequiredService<IAccountTypeService>();
                var allTypes = await accountTypeService.GetAllAsync();
                Assert.Contains(allTypes, t => t.Name == testAccountName);
            });
        }
    }
}
