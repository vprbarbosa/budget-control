using BudgetControl.Contracts.Events;
using BudgetControl.Contracts.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Pwa.Infrastructure.Sync
{
    public sealed class FakeSyncClient : ISyncClient
    {
        public Task<PushEventsResponse> PushAsync(
            IReadOnlyCollection<DomainEventEnvelope> events,
            CancellationToken ct = default)
        {
            return Task.FromResult(new PushEventsResponse
            {
                Accepted = events.Count,
                Duplicates = 0
            });
        }
    }
}
