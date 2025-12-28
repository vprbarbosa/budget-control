using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Application.UseCases.AdjustBudgetCycleCapacity
{
    public sealed class AdjustBudgetCycleCapacityUseCase
    {
        private readonly IBudgetCycleRepository _repository;

        public AdjustBudgetCycleCapacityUseCase(
            IBudgetCycleRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(Guid cycleId, decimal newAmount)
        {
            var cycle = await _repository.GetByIdAsync(cycleId)
                ?? throw new InvalidOperationException("Ciclo não encontrado");

            cycle.AdjustTotalCapacity(Money.FromDecimal(newAmount));

            await _repository.SaveAsync(cycle);
        }
    }
}
