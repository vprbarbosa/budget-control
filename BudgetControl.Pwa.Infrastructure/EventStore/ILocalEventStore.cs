using BudgetControl.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Pwa.Infrastructure.EventStore
{
    public interface ILocalEventStore
    {
        Task AppendAsync(
            IReadOnlyCollection<DomainEventEnvelope> events,
            CancellationToken ct = default);

        Task<IReadOnlyCollection<DomainEventEnvelope>> GetPendingAsync(
            CancellationToken ct = default);

        Task MarkAsSentAsync(
            IReadOnlyCollection<Guid> eventIds,
            CancellationToken ct = default);
    }
}
