using System;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.ValueObjects;
using Lapka.Identity.Infrastructure.Documents;
using Lapka.Identity.Infrastructure.Exceptions;

namespace Lapka.Identity.Infrastructure.Services
{
    public class GrpcPhotoService : IGrpcPhotoService
    {
        private readonly Photo.PhotoClient _client;

        public GrpcPhotoService(Photo.PhotoClient client)
        {
            _client = client;
        }
        
        public async Task<string> GetPhotoPathAsync(Guid photoId, BucketName bucket)
        {
            GetPhotoPathReply response = await _client.GetPhotoPathAsync(new GetPhotoPathRequest
            {
                Id = photoId.ToString(),
                BucketName = bucket.AsGrpcUGet()
            });

            return response.Path;
        }
        public async Task AddAsync(Guid photoId, string name, Stream photo, BucketName bucket)
        {
            await _client.UploadPhotoAsync(new UploadPhotoRequest
            {
                Id = photoId.ToString(),
                Name = name,
                Photo = await ByteString.FromStreamAsync(photo),
                BucketName = bucket.AsGrpcUpload()
            });
        }

        public async Task SetExternalPhotoAsync(Guid photoId, string oldPath, string newPath, BucketName bucket)
        {
            await _client.SetExternalPhotoAsync(new SetExternalPhotoRequest
            {
                Id = photoId.ToString(),
                OldName = oldPath,
                NewName = newPath,
                BucketName = bucket.AsGrpcUploadExternal()
            });
        }

        public async Task DeleteAsync(Guid photoId, BucketName bucket)
        {
            await _client.DeletePhotoAsync(new DeletePhotoRequest
            {
                Id = photoId.ToString(),
                BucketName = bucket.AsGrpcDelete()
            });
        }
        
        
    }
}