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
        public async Task ExportBackup(CancellationToken cancellationToken)
        {
            Response.ContentType = "application/sql";
            Response.Headers.Append("Content-Disposition", "attachment; filename=backup.sql");
            await _postgresBackupService.WriteDumpToStreamAsync(Response.Body, cancellationToken);
        }

        [HttpPost("restore")]
        public async Task<IActionResult> RestoreBackup(CancellationToken cancellationToken)
        {
            await _postgresBackupService.RestoreDumpFromStreamAsync(Request.Body, cancellationToken);
            return Ok(new RestoreResultDto
            {
                Success = true,
                Message = "Database restore completed successfully."
            });
        }
    }
}
