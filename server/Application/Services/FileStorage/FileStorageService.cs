using Microsoft.AspNetCore.Http;
using MoneyManager.Application.Interfaces.FileStorage;
using System;
using System.Threading.Tasks;
using Minio;
using Minio.DataModel.Args;

namespace MoneyManager.Application.Services.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IMinioClient _minio;

        public FileStorageService(IMinioClient minioClient)
        {
            _minio = minioClient;
        }

        public async Task UploadFileAsync(string bucketName, IFormFile file, string key)
        {
            if (file == null)
            {
                return;
            }

            var existsArgs = new BucketExistsArgs().WithBucket(bucketName);
            var hasBucket = await _minio.BucketExistsAsync(existsArgs);

            if (!hasBucket)
            {
                await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
            }

            using var stream = file.OpenReadStream();

            string contentType;
            try
            {
                contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
            }
            catch
            {
                contentType = "application/octet-stream";
            }

            await _minio.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(key)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(contentType));
        }

        public async Task<string> GetFileUrlAsync(string bucketName, string key)
        {
            return await _minio.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(key)
                .WithExpiry(60 * 60));
        }

        public async Task DeleteFileAsync(string bucketName, string key)
        {
            if (string.IsNullOrEmpty(bucketName) || string.IsNullOrEmpty(key))
            {
                return;
            }

            var existsArgs = new BucketExistsArgs().WithBucket(bucketName);
            var hasBucket = await _minio.BucketExistsAsync(existsArgs);
            if (!hasBucket)
            {
                return;
            }

            await _minio.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(key));
        }
    }
}
