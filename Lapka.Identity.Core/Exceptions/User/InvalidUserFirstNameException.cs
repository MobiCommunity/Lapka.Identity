using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.User
{
    public class InvalidUserFirstNameException : DomainException
    {
        public string FirstName { get; }
        public InvalidUserFirstNameException(string firstName) : base($"Invalid first name: {firstName}")
        {
            FirstName = firstName;
        }

        public override string Code => "invalid_first_name";
    }
}