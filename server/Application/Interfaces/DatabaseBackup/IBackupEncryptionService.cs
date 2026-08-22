#nullable enable
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.DatabaseBackup
{
    public interface IBackupEncryptionService
    {
        Task<byte[]> EncryptAsync(byte[] plainData, string password);
        Task<byte[]> DecryptAsync(byte[] encryptedData, string password);
        bool IsEncryptedBackup(byte[] data);
    }
}
