using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.Location
{
    public class LatitudeTooBigException : DomainException
    {
        public string Latitude { get; }
        public LatitudeTooBigException(string latitude) : base($"Latitude is too big: {latitude}")
        {
            Latitude = latitude;
        }

        public override string Code => "Latitude_too_big";
    }
}