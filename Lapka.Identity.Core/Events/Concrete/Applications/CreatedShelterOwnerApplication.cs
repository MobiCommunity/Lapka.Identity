using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Core.Events.Concrete.Applications
{
    public class CreatedShelterOwnerApplication : IDomainEvent
    {
        public ShelterOwnerApplication Application { get; }

        public CreatedShelterOwnerApplication(ShelterOwnerApplication application)
        {
            Application = application;
        }
    }
}