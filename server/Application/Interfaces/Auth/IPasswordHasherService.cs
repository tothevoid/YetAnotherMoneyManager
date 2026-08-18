#nullable enable

namespace MoneyManager.Application.Interfaces.Auth
{
    public enum PasswordVerificationResult
    {
        Failed = 0,
        Success = 1,
        SuccessRehashNeeded = 2
    }

    public interface IPasswordHasherService
    {
        string HashPassword(string password);

        PasswordVerificationResult VerifyHashedPassword(string? hashedPassword, string? providedPassword);
    }
}
