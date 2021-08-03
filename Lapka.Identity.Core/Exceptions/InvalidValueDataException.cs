using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions
{
    public class InvalidValueDataException : DomainException
    {
        public string Field { get; }

        public InvalidValueDataException(string field) : base($"invalid Value {field}")
        {
            Field = field;
        }

        public override string Code => $"invalid_Value_{Field}";
    }
}