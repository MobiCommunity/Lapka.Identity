using Convey.WebApi.Exceptions;
using System;
using System.Net;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Exceptions.Grpc;
using Lapka.Identity.Application.Exceptions.Ownership;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Exceptions.Users;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.Exceptions.Abstract;
using Lapka.Identity.Core.Exceptions.Identity;
using Lapka.Identity.Core.Exceptions.Location;
using Lapka.Identity.Core.Exceptions.Token;
using Lapka.Identity.Core.Exceptions.User;

namespace Lapka.Identity.Infrastructure.Exceptions
{
    public class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                DomainException ex => ex switch
                {
                    InvalidCityValueException invalidCityValueException => new ExceptionResponse(new
                    {
                        code = invalidCityValueException.Code,
                        reason = invalidCityValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidStreetValueException invalidStreetValueException => new ExceptionResponse(new
                    {
                        code = invalidStreetValueException.Code,
                        reason = invalidStreetValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidZipCodeValueException invalidZipCodeValueException => new ExceptionResponse(new
                    {
                        code = invalidZipCodeValueException.Code,
                        reason = invalidZipCodeValueException.Message
                    }, HttpStatusCode.BadRequest),
                    EmailInUseException emailInUseException => new ExceptionResponse(new
                    {
                        code = emailInUseException.Code,
                        reason = emailInUseException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidCredentialsException invalidCredentialsException => new ExceptionResponse(new
                    {
                        code = invalidCredentialsException.Code,
                        reason = invalidCredentialsException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidLatitudeValueException invalidLatitudeValueException => new ExceptionResponse(new
                    {
                        code = invalidLatitudeValueException.Code,
                        reason = invalidLatitudeValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidLongitudeValueException invalidLongitudeValueException => new ExceptionResponse(new
                    {
                        code = invalidLongitudeValueException.Code,
                        reason = invalidLongitudeValueException.Message
                    }, HttpStatusCode.BadRequest),
                    LatitudeIncorrectDataTypeException latitudeIncorrectDataTypeException => new ExceptionResponse(new
                    {
                        code = latitudeIncorrectDataTypeException.Code,
                        reason = latitudeIncorrectDataTypeException.Message
                    }, HttpStatusCode.BadRequest),
                    LatitudeTooBigException latitudeTooBigException => new ExceptionResponse(new
                    {
                        code = latitudeTooBigException.Code,
                        reason = latitudeTooBigException.Message
                    }, HttpStatusCode.BadRequest),
                    LatitudeTooLowException latitudeTooLowException => new ExceptionResponse(new
                    {
                        code = latitudeTooLowException.Code,
                        reason = latitudeTooLowException.Message
                    }, HttpStatusCode.BadRequest),
                    LongitudeIncorrectDataTypeException longitudeIncorrectDataTypeException => new ExceptionResponse(new
                    {
                        code = longitudeIncorrectDataTypeException.Code,
                        reason = longitudeIncorrectDataTypeException.Message
                    }, HttpStatusCode.BadRequest),
                    LongitudeTooBigException longitudeTooBigException => new ExceptionResponse(new
                    {
                        code = longitudeTooBigException.Code,
                        reason = longitudeTooBigException.Message
                    }, HttpStatusCode.BadRequest),
                    LongitudeTooLowException longitudeTooLowException => new ExceptionResponse(new
                    {
                        code = longitudeTooLowException.Code,
                        reason = longitudeTooLowException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidShelterNameException invalidShelterNameException => new ExceptionResponse(new
                    {
                        code = invalidShelterNameException.Code,
                        reason = invalidShelterNameException.Message
                    }, HttpStatusCode.BadRequest),
                    EmptyRefreshTokenException emptyRefreshTokenException => new ExceptionResponse(new
                    {
                        code = emptyRefreshTokenException.Code,
                        reason = emptyRefreshTokenException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidAccessTokenException invalidAccessTokenException => new ExceptionResponse(new
                    {
                        code = invalidAccessTokenException.Code,
                        reason = invalidAccessTokenException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidRefreshTokenException invalidRefreshTokenException => new ExceptionResponse(new
                    {
                        code = invalidRefreshTokenException.Code,
                        reason = invalidRefreshTokenException.Message
                    }, HttpStatusCode.BadRequest),
                    RevokedRefreshTokenException revokedRefreshTokenException => new ExceptionResponse(new
                    {
                        code = revokedRefreshTokenException.Code,
                        reason = revokedRefreshTokenException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidUsernameValueException invalidUsernameValueException => new ExceptionResponse(new
                    {
                        code = invalidUsernameValueException.Code,
                        reason = invalidUsernameValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidUserPhoneNumberException invalidUserPhoneNumberException => new ExceptionResponse(new
                    {
                        code = invalidUserPhoneNumberException.Code,
                        reason = invalidUserPhoneNumberException.Message
                    }, HttpStatusCode.BadRequest),
                    TooLongUserFirstNameException tooLongUserFirstNameException => new ExceptionResponse(new
                    {
                        code = tooLongUserFirstNameException.Code,
                        reason = tooLongUserFirstNameException.Message
                    }, HttpStatusCode.BadRequest),
                    TooLongUserLastNameException tooLongUserLastNameException => new ExceptionResponse(new
                    {
                        code = tooLongUserLastNameException.Code,
                        reason = tooLongUserLastNameException.Message
                    }, HttpStatusCode.BadRequest),
                    TooShortPasswordException tooShortPasswordException => new ExceptionResponse(new
                    {
                        code = tooShortPasswordException.Code,
                        reason = tooShortPasswordException.Message
                    }, HttpStatusCode.BadRequest),
                    TooShortUserFirstNameException tooShortUserFirstNameException => new ExceptionResponse(new
                    {
                        code = tooShortUserFirstNameException.Code,
                        reason = tooShortUserFirstNameException.Message
                    }, HttpStatusCode.BadRequest),
                    TooShortUserLastNameException tooShortUserLastNameException => new ExceptionResponse(new
                    {
                        code = tooShortUserLastNameException.Code,
                        reason = tooShortUserLastNameException.Message
                    }, HttpStatusCode.BadRequest),
                    UsernameTooLongException usernameTooLongException => new ExceptionResponse(new
                    {
                        code = usernameTooLongException.Code,
                        reason = usernameTooLongException.Message
                    }, HttpStatusCode.BadRequest),
                    UsernameTooShortException usernameTooShortException => new ExceptionResponse(new
                    {
                        code = usernameTooShortException.Code,
                        reason = usernameTooShortException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidAggregateIdException invalidAggregateIdException => new ExceptionResponse(new
                    {
                        code = invalidAggregateIdException.Code,
                        reason = invalidAggregateIdException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidBankNumberException invalidBankNumberException => new ExceptionResponse(new
                    {
                        code = invalidBankNumberException.Code,
                        reason = invalidBankNumberException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidBucketNameException invalidBucketNameException => new ExceptionResponse(new
                    {
                        code = invalidBucketNameException.Code,
                        reason = invalidBucketNameException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidEmailValueException invalidEmailValueException => new ExceptionResponse(new
                    {
                        code = invalidEmailValueException.Code,
                        reason = invalidEmailValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidPhoneNumberException invalidPhoneNumberException => new ExceptionResponse(new
                    {
                        code = invalidPhoneNumberException.Code,
                        reason = invalidPhoneNumberException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidValueDataException invalidValueDataException => new ExceptionResponse(new
                    {
                        code = invalidValueDataException.Code,
                        reason = invalidValueDataException.Message
                    }, HttpStatusCode.BadRequest),
                    _ => new ExceptionResponse(new {code = ex.Code, reason = ex.Message},
                        HttpStatusCode.BadRequest),
                },

                AppException ex => ex switch
                {
                    CannotRequestFilesMicroserviceException cannotRequestFilesMicroserviceException =>
                        new ExceptionResponse(new
                        {
                            code = cannotRequestFilesMicroserviceException.Code,
                            reason = cannotRequestFilesMicroserviceException.Message
                        }, HttpStatusCode.InternalServerError),
                    CannotRequestPetsMicroserviceException cannotRequestPetsMicroserviceException =>
                        new ExceptionResponse(new
                        {
                            code = cannotRequestPetsMicroserviceException.Code,
                            reason = cannotRequestPetsMicroserviceException.Message
                        }, HttpStatusCode.InternalServerError),
                    ApplicationForShelterOwnerIsAlreadyMadeException applicationForShelterOwnerIsAlreadyMadeException =>
                        new ExceptionResponse(new
                        {
                            code = applicationForShelterOwnerIsAlreadyMadeException.Code,
                            reason = applicationForShelterOwnerIsAlreadyMadeException.Message
                        }, HttpStatusCode.BadRequest),
                    OwnerApplicationStatusHasToBePendingException ownerApplicationStatusHasToBePendingException =>
                        new ExceptionResponse(new
                        {
                            code = ownerApplicationStatusHasToBePendingException.Code,
                            reason = ownerApplicationStatusHasToBePendingException.Message
                        }, HttpStatusCode.BadRequest),
                    UserAlreadyIsOwnerOfShelterException userAlreadyIsOwnerOfShelterException =>
                        new ExceptionResponse(new
                        {
                            code = userAlreadyIsOwnerOfShelterException.Code,
                            reason = userAlreadyIsOwnerOfShelterException.Message
                        }, HttpStatusCode.BadRequest),
                    UserIsNotOwnerOfShelterException userIsNotOwnerOfShelterException =>
                        new ExceptionResponse(new
                        {
                            code = userIsNotOwnerOfShelterException.Code,
                            reason = userIsNotOwnerOfShelterException.Message
                        }, HttpStatusCode.BadRequest),
                    InvalidShelterIdException invalidShelterIdException =>
                        new ExceptionResponse(new
                        {
                            code = invalidShelterIdException.Code,
                            reason = invalidShelterIdException.Message
                        }, HttpStatusCode.BadRequest),
                    ShelterNotFoundException shelterNotFoundException =>
                        new ExceptionResponse(new
                        {
                            code = shelterNotFoundException.Code,
                            reason = shelterNotFoundException.Message
                        }, HttpStatusCode.NotFound),
                    ShelterOwnerApplicationNotFoundException shelterOwnerApplicationNotFoundException =>
                        new ExceptionResponse(new
                        {
                            code = shelterOwnerApplicationNotFoundException.Code,
                            reason = shelterOwnerApplicationNotFoundException.Message
                        }, HttpStatusCode.NotFound),
                    UserNotFoundException userNotFoundException =>
                        new ExceptionResponse(new
                        {
                            code = userNotFoundException.Code,
                            reason = userNotFoundException.Message
                        }, HttpStatusCode.NotFound),
                    Application.Exceptions.UnauthorizedAccessException unauthorizedAccessException =>
                        new ExceptionResponse(new
                        {
                            code = unauthorizedAccessException.Code,
                            reason = unauthorizedAccessException.Message
                        }, HttpStatusCode.Unauthorized),
                    _ => new ExceptionResponse(
                        new
                        {
                            code = ex.Code,
                            reason = ex.Message
                        },
                        HttpStatusCode.BadRequest)
                },

                _ => new ExceptionResponse(new
                    {
                        code = "error", reason = "There was an error."
                    },
                    HttpStatusCode.BadRequest)
            };
    }
}