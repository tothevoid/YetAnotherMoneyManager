using Microsoft.AspNetCore.Http;
using MoneyManager.Application.Interfaces.FileStorage;

namespace MoneyManager.Application.Tests.Fixtures
{
    public class TestFileStorageService : IFileStorageService
    {
        public Task UploadFile(string bucketName, IFormFile file, string key)
        {
            return Task.CompletedTask;
        }

        public Task<string> GetFileUrl(string bucketName, string key)
        {
            return Task.FromResult($"https://localhost/{bucketName}/{key}");
        }
    }
}
