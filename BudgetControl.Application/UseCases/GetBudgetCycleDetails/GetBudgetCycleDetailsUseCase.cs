using BudgetControl.Application.Abstractions.Clock;
using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Application.UseCases.GetBudgetCycleDetails
{
    public sealed class GetBudgetCycleDetailsUseCase
    {
        private readonly IBudgetCycleRepository _repository;
        private readonly IClock _clock;

        public GetBudgetCycleDetailsUseCase(IBudgetCycleRepository repository, IClock clock)
        {
            _repository = repository;
            _clock = clock;
        }

        public async Task<BudgetCycleDetailsDto> ExecuteAsync(Guid cycleId)
        {
            var cycle = await _repository.GetByIdAsync(cycleId)
                ?? throw new InvalidOperationException("Budget cycle not found.");

            var remaining = cycle.RemainingCapacity.Amount;

            var today = _clock.Today();

            return new BudgetCycleDetailsDto
            {
                Id = cycle.Id,
                FundingSourceName = cycle.Source.Name,
                StartDate = cycle.Period.StartDate,
                EstimatedEndDate = cycle.Period.EstimatedEndDate,

                TotalCapacity = cycle.TotalCapacity.Amount,
                TotalSpent = cycle.TotalSpent.Amount,

                RemainingCapacity = remaining > 0 ? remaining : 0,
                ExceededAmount = remaining < 0 ? -remaining : 0,
                IsOverBudget = cycle.IsOverBudget,

                DailyCapacity = cycle.DailyCapacity(today).Amount,
                RemainingDays = cycle.RemainingDays(today)
            };
        }
    }

}
