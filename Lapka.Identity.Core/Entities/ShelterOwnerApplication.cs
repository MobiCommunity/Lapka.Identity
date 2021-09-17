using System;
using Lapka.Identity.Core.Events.Concrete.Applications;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Core.Entities
{
    public class ShelterOwnerApplication : AggregateRoot
    {
        public Guid ShelterId { get; }
        public Guid UserId { get; }
        public OwnerApplicationStatus Status { get; private set; }
        public DateTime CreationDate { get; }

        public ShelterOwnerApplication(Guid id, Guid shelterId, Guid userId, OwnerApplicationStatus status,
            DateTime creationDate)
        {
            Id = new AggregateId(id);
            ShelterId = shelterId;
            UserId = userId;
            Status = status;
            CreationDate = creationDate;
        }

        public static ShelterOwnerApplication Create(Guid id, Guid shelterId, Guid userId,
            OwnerApplicationStatus status, DateTime creationDate)
        {
            ShelterOwnerApplication application =
                new ShelterOwnerApplication(id, shelterId, userId, status, creationDate);

            application.AddEvent(new CreatedShelterOwnerApplication(application));

            return application;
        }

        public void DeclineApplication()
        {
            Status = OwnerApplicationStatus.Declined;
            AddEvent(new UpdatedShelterOwnerApplication(this));
        }

        public void AcceptApplication()
        {
            Status = OwnerApplicationStatus.Accepted;
            AddEvent(new UpdatedShelterOwnerApplication(this));
        }
    }
}