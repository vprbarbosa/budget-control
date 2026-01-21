using BudgetControl.Domain.Aggregates;
using BudgetControl.Domain.Common;
using BudgetControl.Pwa.Infrastructure.Snapshot;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BudgetControl.Pwa.Snapshot
{
    public sealed class IndexedDbAggregateSnapshotStore<TAggregate>
    : IAggregateSnapshotStore<TAggregate>
    {
        private readonly IJSRuntime _js;

        public IndexedDbAggregateSnapshotStore(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SaveAsync(
            Guid aggregateId,
            TAggregate aggregate,
            CancellationToken ct = default)
        {
            var json = JsonSerializer.Serialize(aggregate);
            await _js.InvokeVoidAsync(
                "saveSnapshot",
                aggregateId.ToString(),
                json);
        }

        public async Task<TAggregate?> LoadAsync(Guid aggregateId, CancellationToken ct = default)
        {
            var json = await _js.InvokeAsync<string?>("loadSnapshot", aggregateId.ToString());
            if (json is null) return default;

            var agg = JsonSerializer.Deserialize<TAggregate>(json);

            if (agg is IRehydratableAggregate rehydratable)
                rehydratable.AfterRehydration();

            return agg;
        }

    }
}
