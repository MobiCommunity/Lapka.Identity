using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions
{
    public class InvalidPhoneNumberException : DomainException
    {
        public string PhoneNumber { get; }
        public InvalidPhoneNumberException(string phoneNumber) : base($"Phone number is not valid {phoneNumber}")
        {
            PhoneNumber = phoneNumber;
        }

        public override string Code => "invalid_phone_number";
    }
}