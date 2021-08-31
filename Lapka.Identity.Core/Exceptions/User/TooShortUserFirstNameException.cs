using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.User
{
    public class TooShortUserFirstNameException : DomainException
    {
        public string FirstName { get; }
        public TooShortUserFirstNameException(string firstName) : base($"Too short first name: {firstName}")
        {
            FirstName = firstName;
        }

        public override string Code => "too_short_first_name";
    }
}