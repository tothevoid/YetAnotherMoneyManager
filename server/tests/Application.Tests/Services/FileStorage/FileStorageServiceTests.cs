using Microsoft.AspNetCore.Http;
using MoneyManager.Application.Interfaces.FileStorage;
using MoneyManager.Tests.Shared.Fixtures;
using System.Text;
using Xunit;

namespace MoneyManager.Application.Tests.Services.FileStorage
{
    [Trait("Category", "S3")]
    public class FileStorageServiceTests : TestBase
    {
        private readonly IFileStorageService _fileStorageService;

        public FileStorageServiceTests(ServiceProviderFixture fixture) : base(fixture)
        {
            _fileStorageService = fixture.CreateFileStorageService();
        }

        [Fact]
        public async Task UploadFileAsync_WhenBucketDoesNotExist_ShouldCreateBucketAndUploadFile()
        {
            var bucketName = $"test-bucket-{Guid.NewGuid():N}";
            var key = $"file-{Guid.NewGuid():N}.txt";
            var formFile = CreateFormFile("Hello MinIO", "test.txt", "text/plain");

            await _fileStorageService.UploadFileAsync(bucketName, formFile, key);

            var url = await _fileStorageService.GetFileUrlAsync(bucketName, key);

            Assert.NotNull(url);
            Assert.Contains(bucketName, url);
            Assert.Contains(key, url);
        }

        [Fact]
        public async Task GetFileStreamAsync_ShouldReturnFileContentDto()
        {
            var bucketName = $"test-bucket-{Guid.NewGuid():N}";
            var key = $"file-{Guid.NewGuid():N}.png";
            var formFile = CreateFormFile("fake-image-bytes", "image.png", "image/png");

            await _fileStorageService.UploadFileAsync(bucketName, formFile, key);

            var fileDto = await _fileStorageService.GetFileStreamAsync(bucketName, key);

            Assert.NotNull(fileDto);
            Assert.NotNull(fileDto.Stream);
            Assert.Equal("image/png", fileDto.ContentType);
            Assert.True(fileDto.Stream.Length > 0);
        }

        [Fact]
        public async Task GetFileUrlAsync_ShouldReturnPresignedUrl()
        {
            var bucketName = $"test-bucket-{Guid.NewGuid():N}";
            var key = $"file-{Guid.NewGuid():N}.png";
            var formFile = CreateFormFile("fake-image-bytes", "image.png", "image/png");

            await _fileStorageService.UploadFileAsync(bucketName, formFile, key);

            var url = await _fileStorageService.GetFileUrlAsync(bucketName, key);

            Assert.False(string.IsNullOrWhiteSpace(url));
            Assert.StartsWith("http", url);
        }

        [Fact]
        public async Task DeleteFileAsync_WhenFileExists_ShouldRemoveObject()
        {
            var bucketName = $"test-bucket-{Guid.NewGuid():N}";
            var key = $"file-{Guid.NewGuid():N}.dat";
            var formFile = CreateFormFile("data-to-delete", "data.dat", "application/octet-stream");

            await _fileStorageService.UploadFileAsync(bucketName, formFile, key);

            var exception = await Record.ExceptionAsync(() => _fileStorageService.DeleteFileAsync(bucketName, key));

            Assert.Null(exception);
        }

        [Fact]
        public async Task DeleteFileAsync_WhenBucketOrKeyIsNullOrEmpty_ShouldHandleGracefully()
        {
            var exception1 = await Record.ExceptionAsync(() => _fileStorageService.DeleteFileAsync(string.Empty, "key"));
            var exception2 = await Record.ExceptionAsync(() => _fileStorageService.DeleteFileAsync("bucket", string.Empty));
            var exception3 = await Record.ExceptionAsync(() => _fileStorageService.DeleteFileAsync($"nonexistent-{Guid.NewGuid():N}", "key"));

            Assert.Null(exception1);
            Assert.Null(exception2);
            Assert.Null(exception3);
        }

        private static IFormFile CreateFormFile(string content, string fileName, string contentType)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);
            return new FormFile(stream, 0, bytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
    }
}
