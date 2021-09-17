using Convey.WebApi.Exceptions;
using System;
using System.Net;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Exceptions.Users;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Infrastructure.Exceptions
{
    public class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                DomainException ex => ex switch
                {
                    InvalidValueDataException invalidValueDataException => new ExceptionResponse(new
                    {
                        code = invalidValueDataException.Code,
                        reason = invalidValueDataException.Message
                    }, HttpStatusCode.NotFound),
                    _ => new ExceptionResponse(new {code = ex.Code, reason = ex.Message},
                        HttpStatusCode.BadRequest),
                },

                AppException ex => ex switch
                {
                    InvalidShelterIdException invalidShelterIdException =>
                        new ExceptionResponse(new
                        {
                            code = invalidShelterIdException.Code,
                            reason = invalidShelterIdException.Message
                        }, HttpStatusCode.BadRequest),
                    UserNotFoundException userNotFoundException =>
                        new ExceptionResponse(new
                        {
                            code = userNotFoundException.Code,
                            reason = userNotFoundException.Message
                        }, HttpStatusCode.NotFound),
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