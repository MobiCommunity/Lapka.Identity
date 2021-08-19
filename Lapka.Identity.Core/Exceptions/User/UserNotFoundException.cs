using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.User
{
    public class UserNotFoundException : DomainException
    {
        public string UserId { get; }
        public UserNotFoundException(string userId) : base($"No user is found: {userId}")
        {
            UserId = userId;
        }

        public override string Code => "user_not_found";
    }
}