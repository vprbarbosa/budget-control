using BudgetControl.Domain.Aggregates;
using BudgetControl.Domain.Entities;
using BudgetControl.Pwa.Infrastructure.Adapters;
using BudgetControl.Pwa.Infrastructure.EventStore;
using BudgetControl.Pwa.Infrastructure.Snapshot;
using BudgetControl.Pwa.Infrastructure.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Pwa.Infrastructure.Application
{
    public sealed class RegisterExpenseUseCase
    {
        private readonly ILocalEventStore _eventStore;
        private readonly ISyncClient _syncClient;
        private readonly IAggregateSnapshotStore<BudgetCycle> _snapshotStore;

        // temporário (no Passo 8 vira estado real do app)
        private static readonly Guid ContextId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private static readonly Guid DeviceId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        private static readonly Guid CycleId = Guid.Parse("33333333-3333-3333-3333-333333333333");

        public RegisterExpenseUseCase(
            IAggregateSnapshotStore<BudgetCycle> snapshotStore,
            ILocalEventStore eventStore,
            ISyncClient syncClient)
        {
            _snapshotStore = snapshotStore;
            _eventStore = eventStore;
            _syncClient = syncClient;
        }

        public async Task ExecuteAsync(
    Guid cycleId,
    decimal amount,
    string? description,
    CancellationToken ct = default)
        {
            Console.WriteLine("=== RegisterExpenseUseCase START ===");
            Console.WriteLine($"[1] Carregando snapshot do ciclo {cycleId}");

            var cycle = await _snapshotStore.LoadAsync(cycleId, ct);

            if (cycle is null)
                throw new InvalidOperationException(
                    $"Cycle {cycleId} não encontrado.");

            Console.WriteLine("[2] Snapshot encontrado");
            Console.WriteLine($"[2.1] Total gasto atual: {cycle.TotalSpent.Amount}");

            Console.WriteLine($"[3] Registrando despesa {amount}");
            cycle.RegisterExpense(amount, description);

            Console.WriteLine($"[3.1] Total gasto após: {cycle.TotalSpent.Amount}");

            Console.WriteLine("[4] Salvando snapshot atualizado");
            await _snapshotStore.SaveAsync(cycleId, cycle, ct);

            Console.WriteLine("[5] Criando evento de sync");
            var evt = new
            {
                CycleId = cycleId,
                Amount = amount,
                Description = description,
                OccurredAt = DateTimeOffset.UtcNow
            };

            var envelope = DomainEventAdapter.Adapt(
                evt,
                ContextId,
                cycleId,
                DeviceId);

            await _eventStore.AppendAsync(new[] { envelope }, ct);

            var pending = await _eventStore.GetPendingAsync(ct);
            await _syncClient.PushAsync(pending, ct);

            await _eventStore.MarkAsSentAsync(
                pending.Select(e => e.EventId).ToArray(),
                ct);

            Console.WriteLine("=== RegisterExpenseUseCase END ===");
        }
    }
}
