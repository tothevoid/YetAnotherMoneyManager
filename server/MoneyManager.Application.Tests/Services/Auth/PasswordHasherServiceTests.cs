using MoneyManager.Application.Interfaces.Auth;
using MoneyManager.Application.Services.Auth;
using Xunit;

namespace MoneyManager.Application.Tests.Services.Auth
{
    [Trait("Category", "Auth")]
    public class PasswordHasherServiceTests
    {
        private readonly IPasswordHasherService _hasher;

        public PasswordHasherServiceTests()
        {
            _hasher = new PasswordHasherService();
        }

        [Fact]
        public void HashPassword_ShouldReturnValidArgon2idFormat()
        {
            var rawPassword = "SecurePassword123!";
            var hash = _hasher.HashPassword(rawPassword);

            Assert.NotNull(hash);
            Assert.StartsWith("$argon2id$v=19$m=65536,t=3,p=2$", hash);
        }

        [Fact]
        public void VerifyHashedPassword_WithCorrectPassword_ShouldReturnSuccess()
        {
            var rawPassword = "CorrectPassword123!";
            var hash = _hasher.HashPassword(rawPassword);

            var result = _hasher.VerifyHashedPassword(hash, rawPassword);

            Assert.Equal(PasswordVerificationResult.Success, result);
        }

        [Fact]
        public void VerifyHashedPassword_WithWrongPassword_ShouldReturnFailed()
        {
            var rawPassword = "CorrectPassword123!";
            var hash = _hasher.HashPassword(rawPassword);

            var result = _hasher.VerifyHashedPassword(hash, "WrongPassword");

            Assert.Equal(PasswordVerificationResult.Failed, result);
        }

        [Fact]
        public void VerifyHashedPassword_WithLegacyPlainText_ShouldReturnSuccessRehashNeeded()
        {
            var plainText = "LegacyPlainTextPassword";

            var result = _hasher.VerifyHashedPassword(plainText, plainText);

            Assert.Equal(PasswordVerificationResult.SuccessRehashNeeded, result);
        }

        [Fact]
        public void VerifyHashedPassword_WithLegacyPlainText_WrongPassword_ShouldReturnFailed()
        {
            var plainText = "LegacyPlainTextPassword";

            var result = _hasher.VerifyHashedPassword(plainText, "DifferentPassword");

            Assert.Equal(PasswordVerificationResult.Failed, result);
        }

        [Fact]
        public void VerifyHashedPassword_BothEmpty_ShouldReturnSuccess()
        {
            var result = _hasher.VerifyHashedPassword(null, "");

            Assert.Equal(PasswordVerificationResult.Success, result);
        }

        [Fact]
        public void VerifyHashedPassword_HashNull_ProvidedNotEmpty_ShouldReturnFailed()
        {
            var result = _hasher.VerifyHashedPassword(null, "somePassword");

            Assert.Equal(PasswordVerificationResult.Failed, result);
        }
    }
}
