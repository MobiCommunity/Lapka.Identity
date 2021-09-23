using System;
using Convey.CQRS.Events;

namespace Lapka.Identity.Application.Events
{
    public class ShelterPhotoRemoved : IEvent
    {
        public string PhotoPath { get; }

        public ShelterPhotoRemoved(string photoPath)
        {
            PhotoPath = photoPath;
        }
    }
}