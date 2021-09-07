using System;
using System.Linq;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Shouldly;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Core.Entities.UserTests
{
    public class UpdateUserPhotoTests
    {
        [Fact]
        public void given_valid_user_photo_id_should_be_updated()
        {
            User user = Extensions.ArrangeUser();
            Guid photoToUpdate = Guid.NewGuid();

            user.UpdatePhoto(photoToUpdate);

            user.ShouldNotBeNull();
            user.PhotoId.ShouldBe(photoToUpdate);
            user.Events.Count().ShouldBe(1);
            IDomainEvent @event = user.Events.Single();
            @event.ShouldBeOfType<UserUpdated>();
        }

    }
}