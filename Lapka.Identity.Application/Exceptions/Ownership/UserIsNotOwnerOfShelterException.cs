using System;

namespace Lapka.Identity.Application.Exceptions.Ownership
{
    public class UserIsNotOwnerOfShelterException : AppException
    {
        public Guid UserId { get; }
        public Guid ShelterId { get; }

        public UserIsNotOwnerOfShelterException(Guid shelterId, Guid userId) : base(
            $"User {userId} is not owner of shelter {shelterId}")
        {
            UserId = userId;
            ShelterId = shelterId;
        }

        public override string Code => "user_is_not_owner";
    }
}