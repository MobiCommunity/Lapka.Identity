using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions
{
    public class InvalidZipCodeValueException : DomainException
    {
        public string Value { get; }
        public InvalidZipCodeValueException(string message) : base($"Invalid Zip Code value of {message}")
        {
            Value = message;
        }

        public override string Code => "invalid_zip_code_value";
    }
}