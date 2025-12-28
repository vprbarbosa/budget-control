using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Application.UseCases.AdjustBudgetCyclePeriod
{
    public sealed class AdjustBudgetCyclePeriodUseCase
    {
        private readonly IBudgetCycleRepository _repository;

        public AdjustBudgetCyclePeriodUseCase(IBudgetCycleRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(AdjustBudgetCyclePeriodInput input)
        {
            // 1. Buscar ciclo
            var cycle = await _repository.GetByIdAsync(input.CycleId)
                ?? throw new InvalidOperationException("Ciclo não encontrado.");

            // 2. Descobrir o último dia fechado
            var lastClosedDay = cycle.Days
                .Where(d => d.IsClosed)
                .OrderByDescending(d => d.Date)
                .FirstOrDefault();

            // 3. Validar encurtamento do período
            if (lastClosedDay is not null &&
                input.EndDate < lastClosedDay.Date)
            {
                throw new InvalidOperationException(
                    "A data final não pode ser anterior ao último dia já fechado.");
            }

            // 4. Delegar ao domínio
            cycle.DefineEndDate(input.EndDate);

            // 5. Persistir
            await _repository.SaveAsync(cycle);
        }
    }

}
