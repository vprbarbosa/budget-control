using BudgetControl.Pwa.Infrastructure.Adapters;
using BudgetControl.Pwa.Infrastructure.EventStore;
using BudgetControl.Pwa.Infrastructure.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Pwa.Services
{
    public sealed class TestEventPipeline
    {
        private readonly ILocalEventStore _store;
        private readonly ISyncClient _sync;

        // Por enquanto hard-coded. No Passo 7 isso vira contexto real do dispositivo/família.
        private static readonly Guid ContextId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private static readonly Guid DeviceId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        public TestEventPipeline(ILocalEventStore store, ISyncClient sync)
        {
            _store = store;
            _sync = sync;
        }

        public async Task<(int pendingAfter, int accepted)> RunOnceAsync(string message, CancellationToken ct = default)
        {
            var testEvent = new BudgetControl.Pwa.Models.TestEvent(message, DateTimeOffset.UtcNow);

            // AggregateId aqui é só simbólico no teste
            var aggregateId = Guid.NewGuid();

            var envelope = DomainEventAdapter.Adapt(
                domainEvent: testEvent,
                contextId: ContextId,
                aggregateId: aggregateId,
                deviceId: DeviceId);

            await _store.AppendAsync(new[] { envelope }, ct);

            var pending = await _store.GetPendingAsync(ct);

            var result = await _sync.PushAsync(pending.ToArray(), ct);

            await _store.MarkAsSentAsync(pending.Select(x => x.EventId).ToArray(), ct);

            var pendingAfter = await _store.GetPendingAsync(ct);

            return (pendingAfter.Count, result.Accepted);
        }

        public async Task<int> GetPendingCountAsync(CancellationToken ct = default)
            => (await _store.GetPendingAsync(ct)).Count;
    }
}
