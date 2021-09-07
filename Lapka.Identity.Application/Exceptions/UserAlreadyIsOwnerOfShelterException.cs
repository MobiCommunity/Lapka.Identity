using System;

namespace Lapka.Identity.Application.Exceptions
{
    public class UserAlreadyIsOwnerOfShelterException : AppException
    {
        public Guid UserId { get; }
        public Guid ShelterId { get; }

        public UserAlreadyIsOwnerOfShelterException(Guid shelterId, Guid userId) : base(
            $"User {userId} is already owner of shelter {shelterId}")
        {
            UserId = userId;
            ShelterId = shelterId;
        }

        public override string Code => "user_is_already_owner";
    }
}