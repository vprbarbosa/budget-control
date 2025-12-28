using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Application.UseCases.GetBudgetCycleDays
{
    public sealed class GetBudgetCycleDaysUseCase
    {
        private readonly IBudgetCycleRepository _repository;

        public GetBudgetCycleDaysUseCase(
            IBudgetCycleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<BudgetCycleDayDto>> ExecuteAsync(Guid cycleId)
        {
            var cycle = await _repository.GetByIdAsync(cycleId)
                ?? throw new InvalidOperationException("Budget cycle not found.");

            var activeDays = cycle.Days
                .Where(d =>
                    d.Date >= cycle.Period.StartDate &&
                    (cycle.EndDate == null || d.Date <= cycle.EndDate.Value));

            return activeDays
                .OrderBy(d => d.Date)
                .Select(d => new BudgetCycleDayDto
                {
                    Date = d.Date,
                    TotalSpent = d.TotalSpent.Amount,
                    IsClosed = d.IsClosed
                })
                .ToList();
        }
    }

}
