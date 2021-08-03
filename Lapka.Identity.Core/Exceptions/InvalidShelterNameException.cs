using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions
{
    public class InvalidShelterNameException : DomainException
    {
        public InvalidShelterNameException(string message) : base(message)
        {
        }

        public override string Code => "invalid_shelter_name";
    }
}