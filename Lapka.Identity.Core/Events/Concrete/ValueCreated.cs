using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Core.Events.Concrete
{
    public class ValueCreated : IDomainEvent
    {
        public Value Value { get; }

        public ValueCreated(Value value)
        {
            Value = value;
        }
    }
}