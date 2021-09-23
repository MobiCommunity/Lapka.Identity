using System;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Grpc;
using Lapka.Identity.Core.ValueObjects;
using Lapka.Identity.Infrastructure.Mongo.Documents;

namespace Lapka.Identity.Infrastructure.Grpc
{
    public class GrpcPhotoService : IGrpcPhotoService
    {
        private readonly PhotoProto.PhotoProtoClient _client;

        public GrpcPhotoService(PhotoProto.PhotoProtoClient client)
        {
            _client = client;
        }

        public async Task<string> AddAsync(string name, Guid userId, bool isPublic, Stream photo, BucketName bucket)
        {
            UploadPhotoReply response = await _client.UploadPhotoAsync(new UploadPhotoRequest
            {
                IsPublic = isPublic,
                Name = name,
                UserId = userId.ToString(),
                Photo = await ByteString.FromStreamAsync(photo),
                BucketName = bucket.AsGrpcUpload()
            });

            return response.Path;
        }

        public async Task DeleteAsync(string photoPath, Guid userId, BucketName bucket)
        {
            await _client.DeletePhotoAsync(new DeletePhotoRequest
            {
                Id = photoPath,
                UserId = userId.ToString(),
                BucketName = bucket.AsGrpcDelete()
            });
        }
    }
}