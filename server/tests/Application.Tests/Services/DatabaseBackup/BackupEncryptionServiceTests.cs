using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Audex.Application.Services.DatabaseBackup;
using Xunit;

namespace Audex.Application.Tests.Services.DatabaseBackup
{
    public class BackupEncryptionServiceTests
    {
        private readonly BackupEncryptionService _service;

        public BackupEncryptionServiceTests()
        {
            _service = new BackupEncryptionService();
        }

        [Fact]
        public async Task EncryptAsync_And_DecryptAsync_WithCorrectPassword_ShouldReturnOriginalData()
        {
            // Arrange
            var originalText = "Hello, this is sensitive financial data for Audex backup!";
            var originalBytes = Encoding.UTF8.GetBytes(originalText);
            var password = "SuperSecretMasterPassword123!@#";

            // Act
            var encryptedBytes = await _service.EncryptAsync(originalBytes, password);
            var decryptedBytes = await _service.DecryptAsync(encryptedBytes, password);
            var decryptedText = Encoding.UTF8.GetString(decryptedBytes);

            // Assert
            Assert.NotNull(encryptedBytes);
            Assert.True(encryptedBytes.Length > originalBytes.Length);
            Assert.True(_service.IsEncryptedBackup(encryptedBytes));
            Assert.Equal(originalText, decryptedText);
        }

        [Fact]
        public async Task DecryptAsync_WithWrongPassword_ShouldThrowCryptographicException()
        {
            // Arrange
            var originalBytes = Encoding.UTF8.GetBytes("Some confidential data");
            var password = "CorrectPassword123!";
            var encryptedBytes = await _service.EncryptAsync(originalBytes, password);

            // Act & Assert
            await Assert.ThrowsAsync<CryptographicException>(async () =>
            {
                await _service.DecryptAsync(encryptedBytes, "WrongPassword456!");
            });
        }

        [Fact]
        public async Task DecryptAsync_WithTamperedCiphertext_ShouldThrowCryptographicException()
        {
            // Arrange
            var originalBytes = Encoding.UTF8.GetBytes("Uncompromised data");
            var password = "MasterPassword123!";
            var encryptedBytes = await _service.EncryptAsync(originalBytes, password);

            // Tamper with one byte in the payload
            encryptedBytes[^1] ^= 0xFF;

            // Act & Assert
            await Assert.ThrowsAsync<CryptographicException>(async () =>
            {
                await _service.DecryptAsync(encryptedBytes, password);
            });
        }

        [Fact]
        public async Task DecryptAsync_WithInvalidHeader_ShouldThrowInvalidDataException()
        {
            // Arrange
            var invalidBytes = new byte[60];
            RandomNumberGenerator.Fill(invalidBytes);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidDataException>(async () =>
            {
                await _service.DecryptAsync(invalidBytes, "AnyPassword");
            });
        }

        [Fact]
        public void IsEncryptedBackup_WithValidHeader_ShouldReturnTrue()
        {
            // Arrange
            var data = new byte[BackupEncryptionService.HeaderSize + 10];
            Array.Copy(BackupEncryptionService.BackupFileHeaderBytes, 0, data, 0, BackupEncryptionService.BackupFileHeaderBytes.Length);

            // Act & Assert
            Assert.True(_service.IsEncryptedBackup(data));
        }

        [Fact]
        public void IsEncryptedBackup_WithInvalidHeaderOrTooShort_ShouldReturnFalse()
        {
            // Arrange
            var shortData = new byte[10];
            var wrongMagic = new byte[60];

            // Act & Assert
            Assert.False(_service.IsEncryptedBackup(null!));
            Assert.False(_service.IsEncryptedBackup(shortData));
            Assert.False(_service.IsEncryptedBackup(wrongMagic));
        }
    }
}
