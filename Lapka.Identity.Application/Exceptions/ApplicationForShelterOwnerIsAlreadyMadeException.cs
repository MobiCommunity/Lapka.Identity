using System;

namespace Lapka.Identity.Application.Exceptions
{
    public class ApplicationForShelterOwnerIsAlreadyMadeException : AppException
    {
        public string UserId { get; }
        public string ShelterId { get; }

        public ApplicationForShelterOwnerIsAlreadyMadeException(string userId, string shelterId) : base(
            $"User {userId} already have made application for owner of {shelterId} shelter")
        {
            UserId = userId;
            ShelterId = shelterId;
        }

        public override string Code => "application_for_shelter_owner_already_made";
    }
}