using BudgetControl.Domain.Aggregates;
using BudgetControl.Pwa.Infrastructure.Snapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Pwa.Infrastructure.Application
{
    public sealed class CloseDayUseCase
    {
        private readonly IAggregateSnapshotStore<BudgetCycle> _snapshotStore;

        public CloseDayUseCase(
            IAggregateSnapshotStore<BudgetCycle> snapshotStore)
        {
            _snapshotStore = snapshotStore;
        }

        public async Task ExecuteAsync(
            Guid cycleId,
            CancellationToken ct = default)
        {
            Console.WriteLine("=== CloseDayUseCase START ===");

            var cycle = await _snapshotStore.LoadAsync(cycleId, ct)
                ?? throw new InvalidOperationException("Cycle not found");

            cycle.CloseCurrentDay();

            await _snapshotStore.SaveAsync(cycleId, cycle, ct);

            Console.WriteLine("[CloseDay] Dia fechado com sucesso");
            Console.WriteLine("=== CloseDayUseCase END ===");
        }
    }
}
