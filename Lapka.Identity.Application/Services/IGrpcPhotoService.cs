using System.IO;
using System.Threading.Tasks;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Services
{
    public interface IGrpcPhotoService
    {
        public Task DeleteAsync(string photoPath, BucketName bucket);
        public Task AddAsync(string photoPath, Stream photo, BucketName bucket);
        
    }
}