using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Exceptions.Ownership
{
    public class OwnerApplicationStatusHasToBePendingException : AppException
    {
        public string Id { get; }
        public OwnerApplicationStatus Status { get; }

        public OwnerApplicationStatusHasToBePendingException(string id, OwnerApplicationStatus status) : base(
            $"Status of {id} application has to be pending, but is it {status}")
        {
            Id = id;
            Status = status;
        }

        public override string Code => "invalid_application_status";
    }
}