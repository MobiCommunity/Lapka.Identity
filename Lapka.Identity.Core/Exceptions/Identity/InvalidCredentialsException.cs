using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.Identity
{
    public class InvalidCredentialsException : DomainException
    {
        public string Credentials { get; }
        public InvalidCredentialsException(string credentials) : base($"Invalid credentials: {credentials}")
        {
            Credentials = credentials;
        }

        public override string Code => "invalid_credentials";
    }
}