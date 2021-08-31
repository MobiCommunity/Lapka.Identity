using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.User
{
    public class UsernameTooShortException : DomainException
    {
        public string Username { get; }
        public UsernameTooShortException(string username) : base($"Username is too short: {username}")
        {
            Username = username;
        }

        public override string Code => "too_short_username";
    }
}