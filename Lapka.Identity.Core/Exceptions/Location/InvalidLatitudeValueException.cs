using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.Location
{
    public class InvalidLatitudeValueException : DomainException
    {
        public string Latitude { get; }
        public InvalidLatitudeValueException(string latitude) : base($"Invalid value of Latitude: {latitude}")
        {
            Latitude = latitude;
        }

        public override string Code => "invalid_latitude_value";
    }
}