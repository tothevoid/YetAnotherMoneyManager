namespace Audex.Application.DTO.DatabaseBackup
{
    public class GeneratedBackupDto
    {
        public byte[] Data { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public bool IsEncrypted { get; set; }
        public long FileSizeBytes => Data?.Length ?? 0;
    }
}
