using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.User
{
    public class TooLongUserLastNameException : DomainException
    {
        public string LastName { get; }
        public TooLongUserLastNameException(string lastName) : base($"Too long last name: {lastName}")
        {
            LastName = lastName;
        }

        public override string Code => "too_long_last_name";
    }
}