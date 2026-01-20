using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Pwa.Infrastructure.Snapshot
{
    public sealed class InMemoryAggregateSnapshotStore<TAggregate>
    : IAggregateSnapshotStore<TAggregate>
    {
        private readonly ConcurrentDictionary<Guid, TAggregate> _store = new();

        public Task<TAggregate?> LoadAsync(
            Guid aggregateId,
            CancellationToken ct = default)
        {
            _store.TryGetValue(aggregateId, out var aggregate);
            return Task.FromResult(aggregate);
        }

        public Task SaveAsync(
            Guid aggregateId,
            TAggregate aggregate,
            CancellationToken ct = default)
        {
            _store[aggregateId] = aggregate;
            return Task.CompletedTask;
        }
    }
}
