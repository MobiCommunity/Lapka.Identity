using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.User
{
    public class InvalidUserLastNameException : DomainException
    {
        public string LastName { get; }
        public InvalidUserLastNameException(string lastName) : base($"Invalid last name: {lastName}")
        {
            LastName = lastName;
        }

        public override string Code => "invalid_last_name";
    }
}