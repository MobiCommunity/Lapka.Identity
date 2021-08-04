using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.Location
{
    public class InvalidLongitudeValueException : DomainException
    {
        public string Longitude { get; }
        public InvalidLongitudeValueException(string longitude) : base($"Invalid value of longitude: {longitude}")
        {
            Longitude = longitude;
        }

        public override string Code => "invalid_longitude_value";
    }
}