using BudgetControl.Contracts.Events;
using BudgetControl.Contracts.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Pwa.Infrastructure.Sync
{
    public interface ISyncClient
    {
        Task<PushEventsResponse> PushAsync(
            IReadOnlyCollection<DomainEventEnvelope> events,
            CancellationToken ct = default);
    }
}
