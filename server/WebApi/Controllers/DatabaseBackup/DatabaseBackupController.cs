#nullable enable
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.DTO.DatabaseBackup;
using MoneyManager.Application.Interfaces.DatabaseBackup;

namespace MoneyManager.WebApi.Controllers.DatabaseBackup
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DatabaseBackupController : ControllerBase
    {
        private readonly IDatabaseBackupService _backupService;

        public DatabaseBackupController(IDatabaseBackupService backupService)
        {
            _backupService = backupService;
        }

        [HttpPost("export")]
        public async Task<IActionResult> ExportBackup([FromBody] ExportBackupRequestDto? request)
        {
            var password = request?.Password;
            var backupBytes = await _backupService.CreateBackupAsync(password);

            var now = DateTime.UtcNow;
            var isEncrypted = !string.IsNullOrEmpty(password);
            var extension = isEncrypted ? "mmbackup" : "sql.gz";
            var fileName = $"moneymanager_backup_{now:yyyyMMdd_HHmmss}.{extension}";

            return File(backupBytes, "application/octet-stream", fileName);
        }

        [HttpPost("validate")]
        public async Task<ActionResult<BackupValidationResultDto>> ValidateBackup(
            [FromForm] IFormFile file,
            [FromForm] string? password = null)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new BackupValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "No backup file uploaded."
                });
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var result = await _backupService.ValidateBackupAsync(fileBytes, password);
            return Ok(result);
        }

        [HttpPost("restore")]
        public async Task<ActionResult<RestoreBackupResultDto>> RestoreBackup(
            [FromForm] IFormFile file,
            [FromForm] string? password = null)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new RestoreBackupResultDto
                {
                    Success = false,
                    Message = "No backup file uploaded."
                });
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var result = await _backupService.RestoreBackupAsync(fileBytes, password);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
