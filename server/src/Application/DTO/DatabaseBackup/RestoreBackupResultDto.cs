#nullable enable

namespace MoneyManager.Application.DTO.DatabaseBackup
{
    public class RestoreBackupResultDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
