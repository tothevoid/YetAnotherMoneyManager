#nullable enable
using System.Threading.Tasks;
using MoneyManager.Application.DTO.DatabaseBackup;

namespace MoneyManager.Application.Interfaces.DatabaseBackup
{
    public interface IDatabaseBackupService
    {
        Task<byte[]> CreateBackupAsync(string? password = null);
        Task<RestoreBackupResultDto> RestoreBackupAsync(byte[] backupData, string? password = null);
        Task<BackupValidationResultDto> ValidateBackupAsync(byte[] backupData, string? password = null);
    }
}
