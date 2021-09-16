using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Application.Commands.Handlers.Shelters;
using Lapka.Identity.Application.Commands.Shelters;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Grpc;
using Lapka.Identity.Application.Services.Repositories;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Events.Concrete.Shelters;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.Exceptions.Location;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Application.Handlers.ShelterTests
{
    public class CreatePetHandlerTests
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly CreateShelterHandler _handler;
        private readonly ILogger<CreateShelterHandler> _logger;
        private readonly IShelterRepository _shelterRepository;

        public CreatePetHandlerTests()
        {
            _shelterRepository = Substitute.For<IShelterRepository>();
            _grpcPhotoService = Substitute.For<IGrpcPhotoService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _logger = Substitute.For<ILogger<CreateShelterHandler>>();
            _handler = new CreateShelterHandler(_logger, _shelterRepository, _grpcPhotoService, _eventProcessor);
        }

        private Task Act(CreateShelter command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_shelter_should_create()
        {
            Guid photoId = Guid.NewGuid();
            Shelter shelter = Extensions.ArrangeShelter(photoId: photoId);
            PhotoFile file = Extensions.ArrangePhotoFile(photoId);
            UserAuth userAuth = Extensions.ArrangeUserAuth();

            CreateShelter command = new CreateShelter(shelter.Id.Value, userAuth, shelter.Name, shelter.PhoneNumber,
                shelter.Email, shelter.Address, shelter.GeoLocation, file, shelter.BankNumber);

            await Act(command);

            await _shelterRepository.Received()
                .AddAsync(Arg.Is<Shelter>(p => p.Id.Value == shelter.Id.Value && p.Name == shelter.Name &&
                                               p.PhoneNumber == shelter.PhoneNumber && p.Email == shelter.Email &&
                                               p.Address.City == shelter.Address.City &&
                                               p.Address.Street == shelter.Address.Street &&
                                               p.Address.ZipCode == shelter.Address.ZipCode &&
                                               p.GeoLocation.Latitude == shelter.GeoLocation.Latitude &&
                                               p.GeoLocation.Longitude == shelter.GeoLocation.Longitude &&
                                               p.PhotoId == photoId));

            await _grpcPhotoService.Received().AddAsync(Arg.Is(photoId), Arg.Is(file.Name), Arg.Is(file.Content),
                Arg.Is(BucketName.ShelterPhotos));
            await _eventProcessor.Received().ProcessAsync(Arg.Is<IEnumerable<IDomainEvent>>(e
                => e.FirstOrDefault().GetType() == typeof(ShelterCreated)));
        }

        [Fact]
        public async Task given_invalid_shelter_name_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            PhotoFile file = Extensions.ArrangePhotoFile();
            UserAuth userAuth = Extensions.ArrangeUserAuth();

            CreateShelter command = new CreateShelter(shelter.Id.Value, userAuth, "", shelter.PhoneNumber,
                shelter.Email, shelter.Address, shelter.GeoLocation, file, shelter.BankNumber);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidShelterNameException>();
        }

        [Fact]
        public async Task given_invalid_shelter_phone_number_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            PhotoFile file = Extensions.ArrangePhotoFile();
            UserAuth userAuth = Extensions.ArrangeUserAuth();

            CreateShelter command = new CreateShelter(shelter.Id.Value, userAuth, shelter.Name, "",
                shelter.Email, shelter.Address, shelter.GeoLocation, file, shelter.BankNumber);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPhoneNumberException>();
        }

        [Fact]
        public async Task given_invalid_shelter_email_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            PhotoFile file = Extensions.ArrangePhotoFile();
            UserAuth userAuth = Extensions.ArrangeUserAuth();

            CreateShelter command = new CreateShelter(shelter.Id.Value, userAuth, shelter.Name, shelter.PhoneNumber,
                "", shelter.Address, shelter.GeoLocation, file, shelter.BankNumber);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidEmailValueException>();
        }

        [Fact]
        public async Task given_invalid_shelter_address_city_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            PhotoFile file = Extensions.ArrangePhotoFile();
            UserAuth userAuth = Extensions.ArrangeUserAuth();

            Exception exception = await Record.ExceptionAsync(async () =>
                await Act(new CreateShelter(shelter.Id.Value, userAuth, shelter.Name, shelter.PhoneNumber,
                    shelter.Email, Extensions.ArrangeAddress(city: ""), shelter.GeoLocation, file,
                    shelter.BankNumber)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidCityValueException>();
        }

        [Fact]
        public async Task given_invalid_shelter_address_name_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            PhotoFile file = Extensions.ArrangePhotoFile();
            UserAuth userAuth = Extensions.ArrangeUserAuth();

            Exception exception = await Record.ExceptionAsync(async () =>
                await Act(new CreateShelter(shelter.Id.Value, userAuth, shelter.Name, shelter.PhoneNumber,
                    shelter.Email, Extensions.ArrangeAddress(street: ""), shelter.GeoLocation, file,
                    shelter.BankNumber)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidStreetValueException>();
        }

        [Fact]
        public async Task given_invalid_shelter_address_zipcode_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            PhotoFile file = Extensions.ArrangePhotoFile();
            UserAuth userAuth = Extensions.ArrangeUserAuth();

            Exception exception = await Record.ExceptionAsync(async () =>
                await Act(new CreateShelter(shelter.Id.Value, userAuth, shelter.Name, shelter.PhoneNumber,
                    shelter.Email, Extensions.ArrangeAddress(zipcode: ""), shelter.GeoLocation, file,
                    shelter.BankNumber)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidZipCodeValueException>();
        }

        [Fact]
        public async Task given_invalid_shelter_location_latitude_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            PhotoFile file = Extensions.ArrangePhotoFile();
            UserAuth userAuth = Extensions.ArrangeUserAuth();

            Exception exception = await Record.ExceptionAsync(async () =>
                await Act(new CreateShelter(shelter.Id.Value, userAuth, shelter.Name, shelter.PhoneNumber,
                    shelter.Email, shelter.Address, Extensions.ArrangeLocation(latitude: ""), file,
                    shelter.BankNumber)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidLatitudeValueException>();
        }

        [Fact]
        public async Task given_too_big_shelter_location_latitude_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            PhotoFile file = Extensions.ArrangePhotoFile();
            UserAuth userAuth = Extensions.ArrangeUserAuth();

            Exception exception = await Record.ExceptionAsync(async () =>
                await Act(new CreateShelter(shelter.Id.Value, userAuth, shelter.Name, shelter.PhoneNumber,
                    shelter.Email, shelter.Address, Extensions.ArrangeLocation(latitude: "90"), file,
                    shelter.BankNumber)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LatitudeTooBigException>();
        }

        [Fact]
        public async Task given_too_low_shelter_location_latitude_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            PhotoFile file = Extensions.ArrangePhotoFile();
            UserAuth userAuth = Extensions.ArrangeUserAuth();

            Exception exception = await Record.ExceptionAsync(async () =>
                await Act(new CreateShelter(shelter.Id.Value, userAuth, shelter.Name, shelter.PhoneNumber,
                    shelter.Email, shelter.Address, Extensions.ArrangeLocation(latitude: "-90"), file,
                    shelter.BankNumber)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LatitudeTooLowException>();
        }

        [Fact]
        public async Task given_invalid_shelter_location_longitude_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            PhotoFile file = Extensions.ArrangePhotoFile();
            UserAuth userAuth = Extensions.ArrangeUserAuth();

            Exception exception = await Record.ExceptionAsync(async () => await Act(new CreateShelter(shelter.Id.Value,
                userAuth,
                shelter.Name, shelter.PhoneNumber,
                shelter.Email, shelter.Address, Extensions.ArrangeLocation(longitude: ""), file, shelter.BankNumber)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidLongitudeValueException>();
        }

        [Fact]
        public async Task given_too_big_shelter_location_longitude_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            PhotoFile file = Extensions.ArrangePhotoFile();
            UserAuth userAuth = Extensions.ArrangeUserAuth();

            Exception exception = await Record.ExceptionAsync(async () => await Act(new CreateShelter(shelter.Id.Value,
                userAuth,
                shelter.Name, shelter.PhoneNumber, shelter.Email, shelter.Address,
                Extensions.ArrangeLocation(longitude: "180"), file, shelter.BankNumber)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LongitudeTooBigException>();
        }

        [Fact]
        public async Task given_too_low_shelter_location_longitude_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            PhotoFile file = Extensions.ArrangePhotoFile();
            UserAuth userAuth = Extensions.ArrangeUserAuth();

            Exception exception = await Record.ExceptionAsync(async () =>
                await Act(new CreateShelter(shelter.Id.Value, userAuth, shelter.Name, shelter.PhoneNumber,
                    shelter.Email, shelter.Address, Extensions.ArrangeLocation(longitude: "-180"), file,
                    shelter.BankNumber)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LongitudeTooLowException>();
        }
    }
}