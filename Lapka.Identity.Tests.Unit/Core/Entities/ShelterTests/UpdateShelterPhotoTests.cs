using System;
using System.Linq;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Events.Concrete.Shelters;
using Shouldly;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Core.Entities.ShelterTests
{
    public class UpdateShelterPhotoTests
    {
        [Fact]
        public void given_valid_shelter_photo_id_should_be_updated()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            string photoToUpdate = "myphoto.jpg";

            shelter.UpdatePhoto(photoToUpdate, "");

            shelter.ShouldNotBeNull();
            shelter.PhotoPath.ShouldBe(photoToUpdate);
            shelter.Events.Count().ShouldBe(1);
            IDomainEvent @event = shelter.Events.Single();
            @event.ShouldBeOfType<ShelterPhotoUpdated>();
        }

    }
}