using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Application.UseCases.GetDayExpenses
{
    public sealed class GetDayExpensesUseCase
    {
        private readonly IBudgetCycleRepository _repository;

        public GetDayExpensesUseCase(
            IBudgetCycleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<DayExpenseDto>> ExecuteAsync(
            Guid cycleId,
            DateOnly date)
        {
            var cycle = await _repository.GetByIdAsync(cycleId)
                ?? throw new InvalidOperationException("Budget cycle not found.");

            var day = cycle.Days
                .FirstOrDefault(d => d.Date == date);

            if (day is null)
                throw new InvalidOperationException("Day not found in cycle.");

            return day.Expenses
                .OrderBy(e => e.CreatedAt)
                .Select(e => new DayExpenseDto
                {
                    Id = e.Id,
                    Amount = e.Amount.Amount,
                    Description = e.Description
                })
                .ToList();
        }
    }

}
