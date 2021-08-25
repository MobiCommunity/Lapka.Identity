using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Commands.Handlers;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.Exceptions.Location;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit;
using File = Lapka.Identity.Core.ValueObjects.File;

namespace Lapka.Identity.Tests.Unit.Application.Handlers
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
            Shelter shelter = ArrangeShelter();
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();

            CreateShelter command = new CreateShelter(shelter.Id.Value, shelter.Name, shelter.PhoneNumber,
                shelter.Email, shelter.Address, shelter.GeoLocation, file, photoId);

            await Act(command);

            string fileNameExpectedValue = $"{photoId:N}.jpg";

            await _shelterRepository.Received()
                .AddAsync(Arg.Is<Shelter>(p => p.Id.Value == shelter.Id.Value && p.Name == shelter.Name &&
                                               p.PhoneNumber == shelter.PhoneNumber && p.Email == shelter.Email &&
                                               p.Address.City == shelter.Address.City &&
                                               p.Address.Street == shelter.Address.Street &&
                                               p.Address.ZipCode == shelter.Address.ZipCode &&
                                               p.GeoLocation.Latitude == shelter.GeoLocation.Latitude &&
                                               p.GeoLocation.Longitude == shelter.GeoLocation.Longitude &&
                                               p.PhotoPath == fileNameExpectedValue));

            await _grpcPhotoService.Received().AddAsync(Arg.Is(fileNameExpectedValue), Arg.Is(file.Content));
            await _eventProcessor.Received().ProcessAsync(Arg.Is<IEnumerable<IDomainEvent>>(e
                => e.FirstOrDefault().GetType() == typeof(ShelterCreated)));
        }

        [Fact]
        public async Task given_invalid_shelter_name_should_throw_an_exception()
        {
            Shelter shelter = ArrangeShelter(name: "");
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();

            CreateShelter command = new CreateShelter(shelter.Id.Value, shelter.Name, shelter.PhoneNumber,
                shelter.Email, shelter.Address, shelter.GeoLocation, file, photoId);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidShelterNameException>();
        }
        
        [Fact]
        public async Task given_invalid_shelter_phone_number_should_throw_an_exception()
        {
            Shelter shelter = ArrangeShelter(phoneNumber: "");
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();

            CreateShelter command = new CreateShelter(shelter.Id.Value, shelter.Name, shelter.PhoneNumber,
                shelter.Email, shelter.Address, shelter.GeoLocation, file, photoId);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPhoneNumberException>();
        }
        
        [Fact]
        public async Task given_invalid_shelter_email_should_throw_an_exception()
        {
            Shelter shelter = ArrangeShelter(email: "");
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();

            CreateShelter command = new CreateShelter(shelter.Id.Value, shelter.Name, shelter.PhoneNumber,
                shelter.Email, shelter.Address, shelter.GeoLocation, file, photoId);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidEmailValueException>();
        }

        [Fact]
        public async Task given_invalid_shelter_address_city_should_throw_an_exception()
        {
            Shelter shelter = ArrangeShelter();
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();
            
            Exception exception = await Record.ExceptionAsync(async () => 
                await Act(new CreateShelter(shelter.Id.Value, shelter.Name, shelter.PhoneNumber,
                shelter.Email, ArrangeAddress(city: ""), shelter.GeoLocation, file, photoId)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidCityValueException>();
        }
        
        [Fact]
        public async Task given_invalid_shelter_address_name_should_throw_an_exception()
        {
            Shelter shelter = ArrangeShelter();
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();
            
            Exception exception = await Record.ExceptionAsync(async () =>
                await Act(new CreateShelter(shelter.Id.Value, shelter.Name, shelter.PhoneNumber,
                shelter.Email, ArrangeAddress(street: ""), shelter.GeoLocation, file, photoId)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidStreetValueException>();
        }
        
        [Fact]
        public async Task given_invalid_shelter_address_zipcode_should_throw_an_exception()
        {
            Shelter shelter = ArrangeShelter();
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();

            Exception exception = await Record.ExceptionAsync(async () => 
                await Act(new CreateShelter(shelter.Id.Value, shelter.Name, shelter.PhoneNumber,
                shelter.Email, ArrangeAddress(zipcode: ""), shelter.GeoLocation, file, photoId)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidZipCodeValueException>();
        }
        
        [Fact]
        public async Task given_invalid_shelter_location_latitude_should_throw_an_exception()
        {
            Shelter shelter = ArrangeShelter();
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();
            
            Exception exception = await Record.ExceptionAsync(async () =>
                await Act(new CreateShelter(shelter.Id.Value, shelter.Name, shelter.PhoneNumber,
                shelter.Email, shelter.Address, ArrangeLocation(latitude: ""), file, photoId)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidLatitudeValueException>();
        }
        
        [Fact]
        public async Task given_too_big_shelter_location_latitude_should_throw_an_exception()
        {
            Shelter shelter = ArrangeShelter();
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();
            
            Exception exception = await Record.ExceptionAsync(async () => 
                await Act(new CreateShelter(shelter.Id.Value, shelter.Name, shelter.PhoneNumber,
                shelter.Email, shelter.Address, ArrangeLocation(latitude: "90"), file, photoId)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LatitudeTooBigException>();
        }
        
        [Fact]
        public async Task given_too_low_shelter_location_latitude_should_throw_an_exception()
        {
            Shelter shelter = ArrangeShelter();
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();

            Exception exception = await Record.ExceptionAsync(async () =>
                await Act(new CreateShelter(shelter.Id.Value, shelter.Name, shelter.PhoneNumber,
                shelter.Email, shelter.Address, ArrangeLocation(latitude: "-90"), file, photoId)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LatitudeTooLowException>();
        }
        
        [Fact]
        public async Task given_invalid_shelter_location_longitude_should_throw_an_exception()
        {
            Shelter shelter = ArrangeShelter();
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();

            Exception exception = await Record.ExceptionAsync(async () => await Act(new CreateShelter(shelter.Id.Value, shelter.Name, shelter.PhoneNumber,
                shelter.Email, shelter.Address, ArrangeLocation(longitude: ""), file, photoId)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidLongitudeValueException>();
        }
        
        [Fact]
        public async Task given_too_big_shelter_location_longitude_should_throw_an_exception()
        {
            Shelter shelter = ArrangeShelter();
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();

            Exception exception = await Record.ExceptionAsync(async () => await Act(new CreateShelter(shelter.Id.Value, shelter.Name, shelter.PhoneNumber,
                shelter.Email, shelter.Address, ArrangeLocation(longitude: "180"), file, photoId)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LongitudeTooBigException>();
        }
        
        [Fact]
        public async Task given_too_low_shelter_location_longitude_should_throw_an_exception()
        {
            Shelter shelter = ArrangeShelter();
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();

            Exception exception = await Record.ExceptionAsync(async () =>
                await Act(new CreateShelter(shelter.Id.Value, shelter.Name, shelter.PhoneNumber,
                shelter.Email, shelter.Address, ArrangeLocation(longitude: "-180"), file, photoId)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LongitudeTooLowException>();
        }
        
        private Shelter ArrangeShelter(AggregateId id = null, string name = null, Address address = null,
            Location location = null, string photoPath = null, string phoneNumber = null, string email = null)
        {
            AggregateId validId = id ?? new AggregateId();
            string validName = name ?? "Miniok";
            Address validAddress = address ?? ArrangeAddress();
            Location validLocation = location ?? ArrangeLocation();
            string validPhotoPath = photoPath ?? $"{Guid.NewGuid()}.jpg";
            string validPhoneNumber = phoneNumber ?? "435731934";
            string validEmail = email ?? "support@lappka.com";

            Shelter shelter = new Shelter(validId.Value, validName, validAddress, validLocation, validPhotoPath,
                validPhoneNumber, validEmail);

            return shelter;
        }

        private Address ArrangeAddress(string street = null, string zipcode = null, string city = null)
        {
            string adressStreet = street ?? "Wojskowa";
            string addressZipcode = zipcode ?? "31-315 Rzeszów";
            string AddressCity = city ?? "Rzeszow";

            Address address = new Address(adressStreet, addressZipcode, AddressCity);

            return address;
        }

        private Location ArrangeLocation(string latitude = null, string longitude = null)
        {
            string shelterAddressLocationLatitude = latitude ?? "22";
            string shelterAddressLocationLongitude = longitude ?? "33";

            Location location = new Location(shelterAddressLocationLatitude, shelterAddressLocationLongitude);

            return location;
        }

        private File ArrangeFile(string name = null, Stream stream = null, string contentType = null)
        {
            string validName = name ?? $"{Guid.NewGuid()}.jpg";
            Stream validStream = stream ?? new MemoryStream();
            string validContentType = contentType ?? "image/jpg";

            File file = new File(validName, validStream, validContentType);

            return file;
        }
    }
}