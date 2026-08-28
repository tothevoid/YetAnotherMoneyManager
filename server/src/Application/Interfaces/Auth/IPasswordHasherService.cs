#nullable enable
using Audex.Application.Enums.Auth;

namespace Audex.Application.Interfaces.Auth
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);

        PasswordVerificationResult VerifyHashedPassword(string? hashedPassword, string? providedPassword);
    }
}
