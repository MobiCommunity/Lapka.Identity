using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Lapka.Identity.Application.Services;

namespace Lapka.Identity.Infrastructure.Services
{
    public class GrpcPhotoService : IGrpcPhotoService
    {
        private readonly Photo.PhotoClient _client;

        public GrpcPhotoService(Photo.PhotoClient client)
        {
            _client = client;
        }
        
        public async Task AddAsync(string photoPath, Stream photo)
        {
            await _client.UploadPhotoAsync(new UploadPhotoRequest
            {
                PhotoPath = photoPath,
                Photo = await ByteString.FromStreamAsync(photo)
            });
        }

        public async Task DeleteAsync(string photoPath)
        {
            await _client.DeletePhotoAsync(new DeletePhotoRequest
            {
                PhotoPath = photoPath
            });
        }
    }
}