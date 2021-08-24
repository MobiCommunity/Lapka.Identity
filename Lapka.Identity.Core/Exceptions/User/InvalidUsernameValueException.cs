using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.User
{
    public class InvalidUsernameValueException : DomainException
    {
        public string Username { get; }
        public InvalidUsernameValueException(string username) : base($"Invalid username: {username}")
        {
            Username = username;
        }

        public override string Code => "invalid_username";
    }
}