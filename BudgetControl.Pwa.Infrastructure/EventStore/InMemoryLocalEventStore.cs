using BudgetControl.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Pwa.Infrastructure.EventStore
{
    public sealed class InMemoryLocalEventStore : ILocalEventStore
    {
        private readonly List<DomainEventEnvelope> _pending = new();

        public Task AppendAsync(
            IReadOnlyCollection<DomainEventEnvelope> events,
            CancellationToken ct = default)
        {
            _pending.AddRange(events);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<DomainEventEnvelope>> GetPendingAsync(
            CancellationToken ct = default)
        {
            return Task.FromResult<IReadOnlyCollection<DomainEventEnvelope>>(
                _pending.ToList());
        }

        public Task MarkAsSentAsync(
            IReadOnlyCollection<Guid> eventIds,
            CancellationToken ct = default)
        {
            _pending.RemoveAll(e => eventIds.Contains(e.EventId));
            return Task.CompletedTask;
        }
    }
}
