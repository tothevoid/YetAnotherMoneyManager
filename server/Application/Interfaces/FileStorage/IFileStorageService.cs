using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MoneyManager.Application.Interfaces.FileStorage
{
    public interface IFileStorageService
    {
        Task UploadFileAsync(string bucketName, IFormFile file, string key);

        Task<string> GetFileUrlAsync(string bucketName, string key);

        Task DeleteFileAsync(string bucketName, string key);
    }
}
