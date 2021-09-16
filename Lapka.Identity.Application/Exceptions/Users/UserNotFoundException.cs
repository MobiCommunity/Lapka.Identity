namespace Lapka.Identity.Application.Exceptions.Users
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