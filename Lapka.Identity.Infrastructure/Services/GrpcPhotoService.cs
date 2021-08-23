using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.ValueObjects;
using Lapka.Identity.Infrastructure.Documents;

namespace Lapka.Identity.Infrastructure.Services
{
    public class GrpcPhotoService : IGrpcPhotoService
    {
        private readonly Photo.PhotoClient _client;

        public GrpcPhotoService(Photo.PhotoClient client)
        {
            _client = client;
        }
        
        public async Task AddAsync(string photoPath, Stream photo, BucketName bucket)
        {
            await _client.UploadPhotoAsync(new UploadPhotoRequest
            {
                PhotoPath = photoPath,
                Photo = await ByteString.FromStreamAsync(photo),
                BucketName = bucket.AsGrpcUpload()
            });
        }

        public async Task DeleteAsync(string photoPath, BucketName bucket)
        {
            await _client.DeletePhotoAsync(new DeletePhotoRequest
            {
                PhotoPath = photoPath,
                BucketName = bucket.AsGrpcDelete()
            });
        }
    }
}