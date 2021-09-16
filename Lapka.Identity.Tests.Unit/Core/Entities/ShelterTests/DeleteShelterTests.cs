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
    public class DeleteShelterTests
    {
        [Fact]
        public void given_valid_shelter_photo_id_should_be_updated()
        {
            Shelter shelter = Extensions.ArrangeShelter();

            shelter.ShouldNotBeNull();
            shelter.Delete();
            
            shelter.Events.Count().ShouldBe(1);
            IDomainEvent @event = shelter.Events.Single();
            @event.ShouldBeOfType<ShelterDeleted>();
        }
    }
}