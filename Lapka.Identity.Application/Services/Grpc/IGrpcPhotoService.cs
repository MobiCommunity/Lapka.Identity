using System;
using System.IO;
using System.Threading.Tasks;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Services.Grpc
{
    public interface IGrpcPhotoService
    {
        public Task<string> AddAsync(string name, Guid userId, bool isPublic, Stream photo, BucketName bucket);
        public Task DeleteAsync(string photoPath, Guid userId, BucketName bucket);
        
    }
}