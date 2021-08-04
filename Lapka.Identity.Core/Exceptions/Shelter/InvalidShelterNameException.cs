using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions
{
    public class InvalidShelterNameException : DomainException
    {
        public string Name { get; }
        
        public InvalidShelterNameException(string name) : base($"Invalid value of {name}")
        {
            Name = name;
        }

        public override string Code => "invalid_shelter_name";
    }
}