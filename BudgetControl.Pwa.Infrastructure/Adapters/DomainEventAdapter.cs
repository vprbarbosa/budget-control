using BudgetControl.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BudgetControl.Pwa.Infrastructure.Adapters
{
    public static class DomainEventAdapter
    {
        public static DomainEventEnvelope Adapt(
            object domainEvent,
            Guid contextId,
            Guid aggregateId,
            Guid deviceId)
        {
            return new DomainEventEnvelope
            {
                EventId = Guid.NewGuid(),
                ContextId = contextId,
                AggregateId = aggregateId,
                AggregateType = domainEvent.GetType().DeclaringType?.Name
                                ?? domainEvent.GetType().Name,
                EventType = domainEvent.GetType().Name,
                OccurredAt = DateTimeOffset.UtcNow,
                DeviceId = deviceId,
                PayloadJson = JsonSerializer.Serialize(domainEvent)
            };
        }
    }
}
