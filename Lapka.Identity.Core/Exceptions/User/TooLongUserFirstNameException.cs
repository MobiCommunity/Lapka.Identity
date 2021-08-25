using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.User
{
    public class TooLongUserFirstNameException : DomainException
    {
        public string LastName { get; }
        public TooLongUserFirstNameException(string lastName) : base($"Too long first name: {lastName}")
        {
            LastName = lastName;
        }

        public override string Code => "too_long_first_name";
    }
}