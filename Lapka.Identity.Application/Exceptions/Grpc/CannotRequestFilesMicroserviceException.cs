using System;

namespace Lapka.Identity.Application.Exceptions.Grpc
{
    public class CannotRequestFilesMicroserviceException : AppException
    {
        public Exception Exception { get; }
        
        public CannotRequestFilesMicroserviceException(Exception exception) 
            : base($"Cannot request files microservice: {exception.Message}")
        {
            Exception = exception;
        }

        public override string Code => "cannot_request_files_microservice";
    }
}