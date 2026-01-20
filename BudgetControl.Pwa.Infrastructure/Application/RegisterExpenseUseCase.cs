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
    decimal amount,
    string? description,
    CancellationToken ct = default)
        {
            Console.WriteLine("=== RegisterExpenseUseCase START ===");

            Console.WriteLine($"[1] Tentando carregar snapshot do ciclo {CycleId}");
            var cycle = await _snapshotStore.LoadAsync(CycleId, ct);

            if (cycle is null)
            {
                Console.WriteLine("[2] Snapshot NÃO encontrado. Criando novo BudgetCycle");

                var source = FundingSource.Create("Fonte Fake");

                cycle = BudgetCycle.Create(
                    source: source,
                    startDate: DateOnly.FromDateTime(DateTime.Today),
                    estimatedDurationInDays: 30,
                    totalCapacity: 1000m);

                Console.WriteLine($"[2.1] Novo BudgetCycle criado com Id {cycle.Id}");
            }
            else
            {
                Console.WriteLine("[2] Snapshot encontrado. Reutilizando BudgetCycle existente");
                Console.WriteLine($"[2.1] Total gasto atual: {cycle.TotalSpent.Amount}");
                Console.WriteLine($"[2.2] Capacidade restante: {cycle.RemainingCapacity.Amount}");
                Console.WriteLine($"[2.3] Dias restantes: {cycle.RemainingDays}");
            }

            Console.WriteLine($"[3] Registrando despesa. Valor={amount}, Desc='{description}'");
            cycle.RegisterExpense(amount, description);

            Console.WriteLine($"[3.1] Total gasto após registro: {cycle.TotalSpent.Amount}");
            Console.WriteLine($"[3.2] Capacidade restante após registro: {cycle.RemainingCapacity.Amount}");

            Console.WriteLine("[4] Salvando snapshot atualizado");
            await _snapshotStore.SaveAsync(CycleId, cycle, ct);

            Console.WriteLine("[5] Criando evento de sync (Application Event)");
            var evt = new
            {
                CycleId,
                Amount = amount,
                Description = description,
                OccurredAt = DateTimeOffset.UtcNow
            };

            var envelope = DomainEventAdapter.Adapt(
                domainEvent: evt,
                contextId: ContextId,
                aggregateId: CycleId,
                deviceId: DeviceId);

            Console.WriteLine($"[5.1] Evento envelopado. EventId={envelope.EventId}");

            Console.WriteLine("[6] Persistindo evento no LocalEventStore");
            await _eventStore.AppendAsync(new[] { envelope }, ct);

            var pending = await _eventStore.GetPendingAsync(ct);
            Console.WriteLine($"[6.1] Eventos pendentes no store: {pending.Count}");

            Console.WriteLine("[7] Executando sync (fake)");
            var result = await _syncClient.PushAsync(pending, ct);

            Console.WriteLine($"[7.1] Sync concluído. Accepted={result.Accepted}, Duplicates={result.Duplicates}");

            Console.WriteLine("[8] Marcando eventos como enviados");
            await _eventStore.MarkAsSentAsync(
                pending.Select(e => e.EventId).ToArray(),
                ct);

            var pendingAfter = await _eventStore.GetPendingAsync(ct);
            Console.WriteLine($"[8.1] Eventos pendentes após limpeza: {pendingAfter.Count}");

            Console.WriteLine("=== RegisterExpenseUseCase END ===");
        }
    }
}
