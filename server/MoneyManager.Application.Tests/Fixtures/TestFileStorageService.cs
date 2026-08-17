using Microsoft.AspNetCore.Http;
using MoneyManager.Application.Interfaces.FileStorage;
using System.Threading.Tasks;

namespace MoneyManager.Application.Tests.Fixtures
{
    //TODO: Use TestContainers minio
    public class TestFileStorageService : IFileStorageService
    {
        public Task UploadFileAsync(string bucketName, IFormFile file, string key)
        {
            return Task.CompletedTask;
        }

        public Task<string> GetFileUrlAsync(string bucketName, string key)
        {
            return Task.FromResult($"https://localhost/{bucketName}/{key}");
        }

        public Task DeleteFileAsync(string bucketName, string key)
        {
            return Task.CompletedTask;
        }
    }
}
