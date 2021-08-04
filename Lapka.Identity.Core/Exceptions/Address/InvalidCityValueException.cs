using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions
{
    public class InvalidCityValueException : DomainException
    {
        public string Value { get; set; }
        public InvalidCityValueException(string value) : base($"Invalid name of city: {value}")
        {
            Value = value;
        }

        public override string Code => "invalid_city_name";
    }
}