using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.Location
{
    public class LatitudeIncorrectDataTypeException : DomainException
    {
        public string Latitude { get; }
        public LatitudeIncorrectDataTypeException(string latitude) : base($"Invalid data type of latitude: {latitude}")
        {
            Latitude = latitude;
        }

        public override string Code => "invalid_latitude_data_type";
    }
}