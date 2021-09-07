using System;

namespace Lapka.Identity.Application.Exceptions
{
    public class ShelterOwnerApplicationNotFoundException : AppException
    {
        public string Id { get; }
        public ShelterOwnerApplicationNotFoundException(string id) : base($"Application does not exists: {id}")
        {
            Id = id;
        }

        public override string Code => "application_does_not_exists";
    }
}