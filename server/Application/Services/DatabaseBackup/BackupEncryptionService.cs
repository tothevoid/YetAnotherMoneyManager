#nullable enable
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;
using MoneyManager.Application.Interfaces.DatabaseBackup;

namespace MoneyManager.Application.Services.DatabaseBackup
{
    public class BackupEncryptionService : IBackupEncryptionService
    {
        // 1. File Header & Format Metadata
        public const string BackupFileHeader = "MMBK";
        public static readonly byte[] BackupFileHeaderBytes = Encoding.UTF8.GetBytes(BackupFileHeader);
        private const byte BackupFormatVersion = 1;
        private const byte EncryptionAlgorithmAesGcm = 1;

        // 2. Symmetric Cipher Parameters (AES-256-GCM)
        private const int KeySize = 32;   // 256-bit key
        private const int NonceSize = 12; // 96-bit AES-GCM IV/Nonce
        private const int TagSize = 16;   // 128-bit authentication tag

        // 3. Key Derivation Parameters (Argon2id)
        private const int SaltSize = 16;             // 128-bit salt
        private const int MemorySize = 65536;        // 64 MB RAM cost
        private const int Iterations = 3;            // Time cost
        private const int DegreeOfParallelism = 4;   // Thread concurrency

        // Total header size calculated dynamically from metadata sizes
        public static readonly int HeaderSize = BackupFileHeaderBytes.Length
            + sizeof(byte) // BackupFormatVersion
            + sizeof(byte) // EncryptionAlgorithmAesGcm
            + SaltSize
            + NonceSize
            + TagSize;

        public bool IsEncryptedBackup(byte[] data)
        {
            if (data == null || data.Length < HeaderSize)
            {
                return false;
            }

            return data.AsSpan(0, BackupFileHeaderBytes.Length).SequenceEqual(BackupFileHeaderBytes);
        }

        public async Task<byte[]> EncryptAsync(byte[] plainData, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be empty for backup encryption.", nameof(password));
            }

            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var nonce = RandomNumberGenerator.GetBytes(NonceSize);
            var tag = new byte[TagSize];
            var ciphertext = new byte[plainData.Length];

            var key = await DeriveKeyAsync(password, salt);

            using (var aesGcm = new AesGcm(key, TagSize))
            {
                aesGcm.Encrypt(nonce, plainData, ciphertext, tag);
            }

            using var memoryStream = new MemoryStream(HeaderSize + ciphertext.Length);
            using var writer = new BinaryWriter(memoryStream);

            writer.Write(BackupFileHeaderBytes);
            writer.Write(BackupFormatVersion);
            writer.Write(EncryptionAlgorithmAesGcm);
            writer.Write(salt);
            writer.Write(nonce);
            writer.Write(tag);
            writer.Write(ciphertext);

            return memoryStream.ToArray();
        }

        public async Task<byte[]> DecryptAsync(byte[] encryptedData, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be empty for backup decryption.", nameof(password));
            }

            if (!IsEncryptedBackup(encryptedData))
            {
                throw new InvalidDataException("The provided file does not have a valid .mmbackup header.");
            }

            using var memoryStream = new MemoryStream(encryptedData);
            using var reader = new BinaryReader(memoryStream);

            var header = reader.ReadBytes(BackupFileHeaderBytes.Length);
            var formatVersion = reader.ReadByte();
            var algorithmType = reader.ReadByte();

            if (algorithmType != EncryptionAlgorithmAesGcm)
            {
                throw new NotSupportedException($"Unsupported encryption algorithm ID: {algorithmType}");
            }

            var salt = reader.ReadBytes(SaltSize);
            var nonce = reader.ReadBytes(NonceSize);
            var tag = reader.ReadBytes(TagSize);
            var ciphertext = reader.ReadBytes((int)(memoryStream.Length - memoryStream.Position));

            var key = await DeriveKeyAsync(password, salt);
            var plainData = new byte[ciphertext.Length];

            try
            {
                using var aesGcm = new AesGcm(key, TagSize);
                aesGcm.Decrypt(nonce, ciphertext, tag, plainData);
            }
            catch (CryptographicException)
            {
                throw new CryptographicException("Decryption failed. The password may be incorrect or the backup file is corrupted.");
            }

            return plainData;
        }

        private static async Task<byte[]> DeriveKeyAsync(string password, byte[] salt)
        {
            return await Task.Run(() =>
            {
                using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
                {
                    Salt = salt,
                    DegreeOfParallelism = DegreeOfParallelism,
                    MemorySize = MemorySize,
                    Iterations = Iterations
                };

                return argon2.GetBytes(KeySize);
            });
        }
    }
}
