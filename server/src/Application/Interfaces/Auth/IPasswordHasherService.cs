#nullable enable
using MoneyManager.Application.Enums.Auth;

namespace MoneyManager.Application.Interfaces.Auth
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);

        PasswordVerificationResult VerifyHashedPassword(string? hashedPassword, string? providedPassword);
    }
}
