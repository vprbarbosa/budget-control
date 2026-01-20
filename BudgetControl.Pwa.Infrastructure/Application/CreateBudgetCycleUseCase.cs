using BudgetControl.Domain.Aggregates;
using BudgetControl.Domain.Entities;
using BudgetControl.Pwa.Infrastructure.Snapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Pwa.Infrastructure.Application
{
    public sealed class CreateBudgetCycleUseCase
    {
        private readonly IAggregateSnapshotStore<BudgetCycle> _snapshotStore;

        public CreateBudgetCycleUseCase(
            IAggregateSnapshotStore<BudgetCycle> snapshotStore)
        {
            _snapshotStore = snapshotStore;
        }

        public async Task<Guid> ExecuteAsync(
            string fundingSourceName,
            DateOnly startDate,
            int estimatedDays,
            decimal totalCapacity,
            CancellationToken ct = default)
        {
            var source = FundingSource.Create(fundingSourceName);

            var cycle = BudgetCycle.Create(
                source,
                startDate,
                estimatedDays,
                totalCapacity);

            await _snapshotStore.SaveAsync(cycle.Id, cycle, ct);

            Console.WriteLine(
                $"[CreateCycle] Cycle criado e salvo. Id={cycle.Id}");

            return cycle.Id;
        }
    }
}
