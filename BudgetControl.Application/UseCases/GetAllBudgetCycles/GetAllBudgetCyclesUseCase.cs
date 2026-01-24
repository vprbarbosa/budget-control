using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BudgetControl.Application.UseCases.GetAllBudgetCycles
{
    public sealed class GetAllBudgetCyclesUseCase
    {
        private readonly IBudgetCycleRepository _repository;

        public GetAllBudgetCyclesUseCase(
            IBudgetCycleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<BudgetCycleListItemDto>> ExecuteAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var cycles = await _repository.GetAllAsync();

            return cycles
                .Where(c =>
                    c.Period.StartDate <= today &&
                    (c.EndDate == null || c.EndDate.Value >= today))
                .Select(c => new BudgetCycleListItemDto
                {
                    Id = c.Id,
                    FundingSourceName = c.Source.Name,
                    Title = $"{c.Source.Name} ({c.Period.StartDate:MM/yyyy})"
                })
                .ToList();
        }
    }
}
