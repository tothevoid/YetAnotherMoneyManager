using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InfrastructureManager.Application.DTO;
using InfrastructureManager.Application.Interfaces;

namespace InfrastructureManager.WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class BackupController : ControllerBase
    {
        private readonly IPostgresBackupService _postgresBackupService;

        public BackupController(IPostgresBackupService postgresBackupService)
        {
            _postgresBackupService = postgresBackupService;
        }

        [HttpGet("backup")]
        public IResult ExportBackup(CancellationToken cancellationToken)
        {
            return Results.Stream(
                async stream => await _postgresBackupService.WriteDumpToStreamAsync(stream, cancellationToken),
                contentType: "application/octet-stream",
                fileDownloadName: "backup.dump");
        }

        [HttpPost("restore")]
        public async Task<IResult> RestoreBackup(CancellationToken cancellationToken)
        {
            await _postgresBackupService.RestoreDumpFromStreamAsync(Request.Body, cancellationToken);
            return Results.Ok(new RestoreResultDto
            {
                Success = true,
                Message = "Database restore completed successfully."
            });
        }
    }
}
