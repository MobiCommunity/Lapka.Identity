using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.User
{
    public class TooShortPasswordException : DomainException
    {
        public TooShortPasswordException() : base("Provided password is too short")
        {
        }

        public override string Code => "too_short_password";
    }
}