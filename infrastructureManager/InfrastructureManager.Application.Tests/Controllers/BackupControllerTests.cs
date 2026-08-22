using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using InfrastructureManager.Application.Interfaces;
using InfrastructureManager.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace InfrastructureManager.Application.Tests.Controllers
{
    public class BackupControllerTests
    {
        private readonly IPostgresBackupService _postgresBackupService;
        private readonly BackupController _controller;

        public BackupControllerTests()
        {
            _postgresBackupService = Substitute.For<IPostgresBackupService>();
            _controller = new BackupController(_postgresBackupService);

            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task ExportBackup_Should_Set_ContentType_And_Call_WriteDumpToStreamAsync()
        {
            // Arrange
            var responseBody = new MemoryStream();
            _controller.HttpContext.Response.Body = responseBody;

            // Act
            await _controller.ExportBackup(CancellationToken.None);

            // Assert
            _controller.HttpContext.Response.ContentType.Should().Be("application/sql");
            await _postgresBackupService.Received(1).WriteDumpToStreamAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task RestoreBackup_Should_Call_RestoreDumpFromStreamAsync_And_Return_Ok()
        {
            // Arrange
            var requestBody = new MemoryStream(new byte[] { 1, 2, 3 });
            _controller.HttpContext.Request.Body = requestBody;

            // Act
            var result = await _controller.RestoreBackup(CancellationToken.None);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            await _postgresBackupService.Received(1).RestoreDumpFromStreamAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>());
        }
    }
}
