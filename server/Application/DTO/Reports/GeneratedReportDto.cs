namespace MoneyManager.Application.DTO.Reports
{
    public class GeneratedReportDto
    {
        public byte[] Data { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSizeBytes => Data?.Length ?? 0;
    }
}
