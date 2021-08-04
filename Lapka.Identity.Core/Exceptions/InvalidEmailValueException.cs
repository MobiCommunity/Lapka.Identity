using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions
{
    public class InvalidEmailValueException : DomainException
    {
        public string Email { get; }
        public InvalidEmailValueException(string email) : base($"Email is not valid {email}")
        {
            Email = email;
        }

        public override string Code => "invalid_email";
    }
}