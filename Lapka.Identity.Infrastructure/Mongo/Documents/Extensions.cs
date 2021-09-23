using System.Linq;
using GeoCoordinatePortable;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Dto.Shelters;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static ShelterDocument AsDocument(this Shelter shelter)
        {
            return new ShelterDocument
            {
                Id = shelter.Id.Value,
                Name = shelter.Name,
                Address = shelter.Address.AsDocument(),
                GeoLocation = shelter.GeoLocation.AsDocument(),
                PhotoPath = shelter.PhotoPath,
                Email = shelter.Email.Value,
                PhoneNumber = shelter.PhoneNumber.Value,
                BankNumber = shelter.BankNumber.Value,
                Owners = shelter.Owners,
                IsDeleted = shelter.IsDeleted
            };
        }

        public static LocationDocument AsDocument(this Location shelter)
        {
            return new LocationDocument
            {
                Latitude = shelter.Latitude.AsDouble(),
                Longitude = shelter.Longitude.AsDouble()
            };
        }

        public static AddressDocument AsDocument(this Address shelter)
        {
            return new AddressDocument
            {
                Street = shelter.Street,
                City = shelter.City,
                ZipCode = shelter.ZipCode
            };
        }

        public static Shelter AsBusiness(this ShelterDocument shelter)
        {
            return new Shelter(shelter.Id, shelter.Name, shelter.Address.AsBusiness(), shelter.GeoLocation.AsBusiness(),
                new PhoneNumber(shelter.PhoneNumber), new EmailAddress(shelter.Email),
                new BankNumber(shelter.BankNumber), shelter.PhotoPath, shelter.IsDeleted, shelter.Owners);
        }

        public static ShelterBasicDto AsDto(this ShelterDocument shelter)
        {
            return new ShelterBasicDto
            {
                Id = shelter.Id,
                Address = shelter.Address.AsDto(),
                PhotoPath = shelter.PhotoPath,
                Name = shelter.Name
            };
        }

        public static ShelterDto AsDto(this ShelterDocument shelter, string latitude, string longitude)
        {
            double? distance = null;
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                Location location = new Location(latitude, longitude);
                GeoCoordinate pin1 = new GeoCoordinate(shelter.GeoLocation.Latitude,
                    shelter.GeoLocation.Longitude);
                GeoCoordinate pin2 = new GeoCoordinate(location.Latitude.AsDouble(), location.Longitude.AsDouble());
                distance = pin1.GetDistanceTo(pin2);
            }

            return new ShelterDto
            {
                Id = shelter.Id,
                Address = shelter.Address.AsDto(),
                Email = shelter.Email,
                GeoLocation = shelter.GeoLocation.AsDto(),
                PhotoPath = shelter.PhotoPath,
                Name = shelter.Name,
                PhoneNumber = shelter.PhoneNumber,
                Distance = distance,
                BankNumber = shelter.BankNumber
            };
        }

        public static ShelterDto AsDetailDto(this ShelterDocument shelter)
        {
            return new ShelterDto
            {
                Id = shelter.Id,
                Address = shelter.Address.AsDto(),
                Email = shelter.Email,
                GeoLocation = shelter.GeoLocation.AsDto(),
                PhotoPath = shelter.PhotoPath,
                Name = shelter.Name,
                PhoneNumber = shelter.PhoneNumber,
                BankNumber = shelter.BankNumber
            };
        }

        public static AddressDto AsDto(this AddressDocument address)
        {
            return new AddressDto
            {
                Street = address.Street,
                City = address.City,
                ZipCode = address.ZipCode
            };
        }

        public static LocationDto AsDto(this LocationDocument location)
        {
            return new LocationDto
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
        }

        public static Location AsBusiness(this LocationDocument shelter)
        {
            return new Location(shelter.Latitude.ToString(), shelter.Longitude.ToString());
        }

        public static Address AsBusiness(this AddressDocument shelter)
        {
            return new Address(shelter.Street, shelter.ZipCode, shelter.City);
        }

        public static UserDocument AsDocument(this User user)
        {
            return new UserDocument
            {
                Id = user.Id.Value,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email.Value,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber?.Value,
                PhotoPath = user.PhotoPath,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
            };
        }

        public static User AsBusiness(this UserDocument user)
        {
            return new User(user.Id, user.Username, user.FirstName, user.LastName, new EmailAddress(user.Email),
                user.Password, user.CreatedAt, user.Role, new PhoneNumber(user.PhoneNumber), user.PhotoPath);
        }

        public static UserDto AsDto(this UserDocument user)
        {
            return new UserDto
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PhotoPath = user.PhotoPath,
                Role = user.Role,
                Username = user.Username,
            };
        }

        public static UserDto AsDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id.Value,
                CreatedAt = user.CreatedAt,
                Email = user.Email.Value,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber?.Value,
                PhotoPath = user.PhotoPath,
                Role = user.Role,
                Username = user.Username,
            };
        }

        public static ShelterOwnerApplicationDocument AsDocument(this ShelterOwnerApplication application)
        {
            return new ShelterOwnerApplicationDocument
            {
                Id = application.Id.Value,
                ShelterId = application.ShelterId,
                UserId = application.UserId,
                Status = application.Status,
                CreationDate = application.CreationDate
            };
        }

        public static ShelterOwnerApplicationDto AsDto(this ShelterOwnerApplicationDocument application,
            ShelterBasicDto shelter, UserDto user)
        {
            return new ShelterOwnerApplicationDto
            {
                Id = application.Id,
                Shelter = shelter,
                User = user,
                Status = application.Status,
                CreationDate = application.CreationDate
            };
        }

        public static ShelterOwnerApplication AsBusiness(this ShelterOwnerApplicationDocument application)
        {
            return new ShelterOwnerApplication(application.Id, application.ShelterId, application.UserId,
                application.Status, application.CreationDate);
        }

        public static RefreshToken AsBusiness(this JsonWebTokenDocument token)
        {
            return new RefreshToken(token.Id, token.UserId, token.RefreshToken, token.CreatedAt, token.ExpiresAt);
        }

        public static JsonWebTokenDocument AsDocument(this RefreshToken token)
        {
            return new JsonWebTokenDocument
            {
                Id = token.Id.Value,
                UserId = token.UserId,
                RefreshToken = token.Token,
                CreatedAt = token.CreatedAt,
                ExpiresAt = token.RevokedAt
            };
        }

        public static ShelterViewsDocument AsDocument(this ShelterViews views)
        {
            return new ShelterViewsDocument
            {
                Id = views.Id,
                ActualMonthViewsCount = views.ViewsCount,
                PreviousMonthsViews = views.PreviousMonthsViews.Select(x => x.AsDocument())
            };
        }
        
        public static ShelterViewsDto AsDto(this ShelterViewsDocument views)
        {
            return new ShelterViewsDto
            {
                Id = views.Id,
                ActualMonthViewsCount = views.ActualMonthViewsCount,
                PreviousMonthsViews = views.PreviousMonthsViews.Select(x => x.AsDto())
            };
        }
        
        public static ShelterViews AsBusiness(this ShelterViewsDocument views)
        {
            return new ShelterViews(views.Id, views.ActualMonthViewsCount,
                views.PreviousMonthsViews.Select(x => x.AsBusiness()));
        }

        public static ViewHistory AsBusiness(this ViewHistoryDocument history)
        {
            return new ViewHistory(history.MonthOfTheYear, history.Year, history.Views);
        }
        
        public static ViewHistoryDocument AsDocument(this ViewHistory history)
        {
            return new ViewHistoryDocument
            {
                MonthOfTheYear = history.MonthOfTheYear,
                Year = history.Year,
                Views = history.Views
            };
        }
        
        public static ViewHistoryDto AsDto(this ViewHistoryDocument history)
        {
            return new ViewHistoryDto
            {
                MonthOfTheYear = history.MonthOfTheYear,
                Year = history.Year,
                Views = history.Views
            };
        }

        public static UploadPhotoRequest.Types.Bucket AsGrpcUpload(this BucketName bucket)
        {
            return bucket switch
            {
                BucketName.PetPhotos => UploadPhotoRequest.Types.Bucket.PetPhotos,
                BucketName.ShelterPhotos => UploadPhotoRequest.Types.Bucket.ShelterPhotos,
                BucketName.UserPhotos => UploadPhotoRequest.Types.Bucket.UserPhotos,
                _ => throw new InvalidBucketNameException(bucket.ToString())
            };
        }

        public static DeletePhotoRequest.Types.Bucket AsGrpcDelete(this BucketName bucket)
        {
            return bucket switch
            {
                BucketName.PetPhotos => DeletePhotoRequest.Types.Bucket.PetPhotos,
                BucketName.ShelterPhotos => DeletePhotoRequest.Types.Bucket.ShelterPhotos,
                BucketName.UserPhotos => DeletePhotoRequest.Types.Bucket.UserPhotos,
                _ => throw new InvalidBucketNameException(bucket.ToString())
            };
        }
    }
}