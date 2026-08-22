#nullable enable

namespace MoneyManager.Application.DTO.DatabaseBackup
{
    public class BackupValidationResultDto
    {
        public bool IsValid { get; set; }
        public bool IsEncrypted { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
