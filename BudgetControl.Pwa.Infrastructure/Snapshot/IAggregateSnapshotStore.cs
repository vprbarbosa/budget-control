using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Pwa.Infrastructure.Snapshot
{
    public interface IAggregateSnapshotStore<TAggregate>
    {
        Task<TAggregate?> LoadAsync(
            Guid aggregateId,
            CancellationToken ct = default);

        Task SaveAsync(
            Guid aggregateId,
            TAggregate aggregate,
            CancellationToken ct = default);
    }
}
