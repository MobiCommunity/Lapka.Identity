using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Core.Events.Concrete.Applications
{
    public class UpdatedShelterOwnerApplication : IDomainEvent
    {
        public ShelterOwnerApplication Application { get; }

        public UpdatedShelterOwnerApplication(ShelterOwnerApplication application)
        {
            Application = application;
        }
    }
}