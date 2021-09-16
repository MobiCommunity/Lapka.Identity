using System;
using System.IO;
using System.Threading.Tasks;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Services.Grpc
{
    public interface IGrpcPhotoService
    {
        public Task<string> GetPhotoPathAsync(Guid photoId, BucketName bucket);
        public Task DeleteAsync(Guid photoId, BucketName bucket);
        public Task AddAsync(Guid photoId, string name, Stream photo, BucketName bucket);
        public Task SetExternalPhotoAsync(Guid photoId, string oldPath, string newPath, BucketName bucket);
        
    }
}