using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.User
{
    public class TooShortUserLastNameException : DomainException
    {
        public string LastName { get; }
        public TooShortUserLastNameException(string lastName) : base($"Too short last name: {lastName}")
        {
            LastName = lastName;
        }

        public override string Code => "too_short_last_name";
    }
}