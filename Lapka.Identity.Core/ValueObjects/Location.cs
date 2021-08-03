using Lapka.Identity.Core.Exceptions.Location;

namespace Lapka.Identity.Core.ValueObjects
{
    public class Location
    {
        public string Latitude  { get; }
        public string Longitude   { get; }

        public Location(string latitude, string longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            
            Validate();
        }

        private void Validate()
        {
            ValidateLatitude();
            ValidateLongitude();
        }

        private void ValidateLongitude()
        {
            if (string.IsNullOrWhiteSpace(Longitude))
            {
                throw new InvalidLongitudeValueException(Longitude);
            }
            
            if (int.Parse(Longitude) < -180)
            {
                throw new LongitudeTooLowException(Longitude);
            }

            if (int.Parse(Longitude) > 180)
            {
                throw new LongitudeTooBigException(Longitude);
            }
        }

        private void ValidateLatitude()
        {
            if (string.IsNullOrWhiteSpace(Latitude))
            {
                throw new InvalidLatitudeValueException(Latitude);
            }
            
            if (int.Parse(Latitude) < -90)
            {
                throw new LatitudeTooLowException(Latitude);
            }

            if (int.Parse(Latitude) > 90)
            {
                throw new LatitudeTooBigException(Latitude);
            }
        }
    }
}