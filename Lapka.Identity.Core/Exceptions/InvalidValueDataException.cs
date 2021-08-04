using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions
{
    public class InvalidValueDataException : DomainException
    {
        public string Field { get; }

        public InvalidValueDataException(string field) : base($"Invalid value of {field}")
        {
            Field = field;
        }

        public override string Code => $"invalid_value_data";
    }
}