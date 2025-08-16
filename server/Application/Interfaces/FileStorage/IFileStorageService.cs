using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MoneyManager.Application.Interfaces.FileStorage
{
    public interface IFileStorageService
    {
        Task UploadFile(string bucketName, IFormFile file, string key);

        Task<string> GetFileUrl(string bucketName, string key);
    }
}
