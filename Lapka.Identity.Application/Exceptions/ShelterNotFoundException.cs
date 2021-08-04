namespace Lapka.Identity.Application.Exceptions
{
    public class ShelterNotFoundException : AppException
    {
        public ShelterNotFoundException() : base("shelter not exists in database")
        {
        }

        public override string Code => "shelter_not_found";
    }
}