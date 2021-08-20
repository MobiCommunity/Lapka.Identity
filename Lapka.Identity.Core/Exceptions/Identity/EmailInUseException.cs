using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.Identity
{
    public class EmailInUseException : DomainException
    {
        public string Email { get; set; }

        public EmailInUseException(string email) : base($"Email is already taken: {email}")
        {
            Email = email;
        }

        public override string Code => "email_is_taken";
    }
}