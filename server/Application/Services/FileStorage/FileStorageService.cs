using Microsoft.AspNetCore.Http;
using MoneyManager.Application.Interfaces.FileStorage;
using System;
using System.Linq;
using System.Threading.Tasks;
using Minio;
using Minio.DataModel.Args;
using MoneyManager.Infrastructure.Migrations;

namespace MoneyManager.Application.Services.FileStorage
{
    public class FileStorageService: IFileStorageService
    {
        private readonly IMinioClient _minio;

        public FileStorageService(IMinioClient minioClient)
        {
            _minio = minioClient;

           
        }

        public async Task UploadFile(string bucketName, IFormFile file, string key)
        {
            var existsArgs = new BucketExistsArgs().WithBucket(bucketName);
            var hasBucket = await _minio.BucketExistsAsync(existsArgs);

            if (!hasBucket)
            {
                await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
            }

            using var stream = file.OpenReadStream();

            await _minio.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(key)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType));
        }

        public async Task<string> GetFileUrl(string bucketName, string key)
        {
            return await _minio.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(key)
                .WithExpiry(60 * 60));
        }

        public async Task DeleteFile(string bucketName, string key)
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
