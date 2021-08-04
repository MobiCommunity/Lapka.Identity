using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.Location
{
    public class LatitudeTooLowException : DomainException
    {
        public string Latitude { get; }
        public LatitudeTooLowException(string latitude) : base($"Latitude is too low: {latitude}")
        {
            Latitude = latitude;
        }

        public override string Code => "Latitude_too_low";
    }
}