namespace Lapka.Identity.Application.Exceptions
{
    public class ShelterNotFoundException : AppException
    {
        public string Id { get; }
        public ShelterNotFoundException(string id) : base($"shelter not found: {id}")
        {
            Id = id;
        }

        public override string Code => "shelter_not_found";
    }
}