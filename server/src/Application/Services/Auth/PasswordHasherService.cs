#nullable enable
using System;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using Audex.Application.Enums.Auth;
using Audex.Application.Interfaces.Auth;

namespace Audex.Application.Services.Auth
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private const int Version = 19;
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int DegreeOfParallelism = 2;
        private const int MemorySize = 65536; // 64 MB
        private const int Iterations = 3;

        private static readonly string Argon2idPrefix = $"$argon2id$v={Version}$m={MemorySize},t={Iterations},p={DegreeOfParallelism}$";

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }

            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = ComputeArgon2idHash(password, salt);

            var saltBase64 = Convert.ToBase64String(salt);
            var hashBase64 = Convert.ToBase64String(hash);

            return $"{Argon2idPrefix}{saltBase64}${hashBase64}";
        }

        public PasswordVerificationResult VerifyHashedPassword(string? hashedPassword, string? providedPassword)
        {
            // Empty password handling
            if (string.IsNullOrEmpty(hashedPassword))
            {
                return string.IsNullOrEmpty(providedPassword)
                    ? PasswordVerificationResult.Success
                    : PasswordVerificationResult.Failed;
            }

            if (string.IsNullOrEmpty(providedPassword))
            {
                return PasswordVerificationResult.Failed;
            }

            // Argon2id format verification
            if (hashedPassword.StartsWith(Argon2idPrefix, StringComparison.Ordinal))
            {
                var payload = hashedPassword[Argon2idPrefix.Length..];
                var parts = payload.Split('$');
                if (parts.Length != 2)
                {
                    return PasswordVerificationResult.Failed;
                }

                try
                {
                    var salt = Convert.FromBase64String(parts[0]);
                    var expectedHash = Convert.FromBase64String(parts[1]);

                    var computedHash = ComputeArgon2idHash(providedPassword, salt);

                    return CryptographicOperations.FixedTimeEquals(computedHash, expectedHash)
                        ? PasswordVerificationResult.Success
                        : PasswordVerificationResult.Failed;
                }
                catch
                {
                    return PasswordVerificationResult.Failed;
                }
            }

            // Legacy plain-text fallback with auto-rehash migration trigger
            if (string.Equals(hashedPassword, providedPassword, StringComparison.Ordinal))
            {
                return PasswordVerificationResult.SuccessRehashNeeded;
            }

            return PasswordVerificationResult.Failed;
        }

        private static byte[] ComputeArgon2idHash(string password, byte[] salt)
        {
            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = DegreeOfParallelism,
                MemorySize = MemorySize,
                Iterations = Iterations
            };

            return argon2.GetBytes(HashSize);
        }
    }
}
