using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.User
{
    public class InvalidUserPhoneNumberException : DomainException
    {
        public string PhoneNumber { get; }
        public InvalidUserPhoneNumberException(string phoneNumber) : base($"Invalid phone number: {phoneNumber}")
        {
            PhoneNumber = phoneNumber;
        }

        public override string Code => "invalid_phone_number";
    }
}