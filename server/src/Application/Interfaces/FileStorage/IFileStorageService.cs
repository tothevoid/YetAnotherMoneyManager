using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MoneyManager.Application.DTO.FileStorage;

namespace MoneyManager.Application.Interfaces.FileStorage
{
    public interface IFileStorageService
    {
        Task UploadFileAsync(string bucketName, IFormFile file, string key);

        Task UploadBytesAsync(string bucketName, byte[] data, string key, string contentType);

        Task<FileStreamDto> GetFileStreamAsync(string bucketName, string key);

        Task<string> GetFileUrlAsync(string bucketName, string key);

        Task DeleteFileAsync(string bucketName, string key);
    }
}
