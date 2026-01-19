using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Contracts.Events
{
    public sealed class DomainEventEnvelope
    {
        public Guid EventId { get; init; } = Guid.NewGuid();

        public Guid ContextId { get; init; }

        public Guid AggregateId { get; init; }

        public string AggregateType { get; init; } = default!;

        public string EventType { get; init; } = default!;

        public DateTimeOffset OccurredAt { get; init; }

        public Guid DeviceId { get; init; }

        public string PayloadJson { get; init; } = default!;

        public int Version { get; init; } = 1;
    }
}
