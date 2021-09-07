using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.User
{
    public class UsernameTooLongException : DomainException
    {
        public string Username { get; }
        public UsernameTooLongException(string username) : base($"Username is too long: {username}")
        {
            Username = username;
        }

        public override string Code => "too_long_username";
    }
}