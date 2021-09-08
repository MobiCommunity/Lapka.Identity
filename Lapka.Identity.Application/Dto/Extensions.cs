using GeoCoordinatePortable;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Dto
{
    public static class Extensions
    {
        public static string GetFileExtension(this File file) =>
            file.Name.Contains('.') ? file.Name.Split('.')[1] : string.Empty;
    }
}