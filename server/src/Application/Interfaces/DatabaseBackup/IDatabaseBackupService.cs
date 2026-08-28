#nullable enable
using System.Threading.Tasks;
using Audex.Application.DTO.DatabaseBackup;

namespace Audex.Application.Interfaces.DatabaseBackup
{
    public interface IDatabaseBackupService
    {
        Task<GeneratedBackupDto> CreateBackupAsync(string? password = null);
        Task<RestoreBackupResultDto> RestoreBackupAsync(byte[] backupData, string? password = null);
        Task<BackupValidationResultDto> ValidateBackupAsync(byte[] backupData, string? password = null);
    }
}
