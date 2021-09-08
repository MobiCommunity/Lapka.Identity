using System;

namespace Lapka.Identity.Core.ValueObjects
{
    public class ShelterOwnerApplication
    {
        public Guid Id { get; }
        public Guid ShelterId { get; }
        public Guid UserId { get; }
        public OwnerApplicationStatus Status { get; private set; }
        public DateTime CreationDate { get; }

        public ShelterOwnerApplication(Guid id, Guid shelterId, Guid userId, OwnerApplicationStatus status,
            DateTime creationDate)
        {
            Id = id;
            ShelterId = shelterId;
            UserId = userId;
            Status = status;
            CreationDate = creationDate;
        }

        public void DeclineApplication()
        {
            Status = OwnerApplicationStatus.Declined;
        }
        
        public void AcceptApplication()
        {
            Status = OwnerApplicationStatus.Accepted;
        }
    }
}