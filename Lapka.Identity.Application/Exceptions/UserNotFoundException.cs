using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Application.Exceptions
{
    public class UserNotFoundException : AppException
    {
        public string UserId { get; }
        public UserNotFoundException(string userId) : base($"No user is found: {userId}")
        {
            UserId = userId;
        }

        public override string Code => "user_not_found";
    }
}