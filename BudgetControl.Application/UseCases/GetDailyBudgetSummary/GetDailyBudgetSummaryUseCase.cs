using BudgetControl.Application.Abstractions.Clock;
using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Application.DTOs;

namespace BudgetControl.Application.UseCases.GetDailyBudgetSummary
{
    public sealed class GetDailyBudgetSummaryUseCase
    {
        private readonly IBudgetCycleRepository _cycleRepository;

        public GetDailyBudgetSummaryUseCase(
            IBudgetCycleRepository cycleRepository)
        {
            _cycleRepository = cycleRepository;
        }

        public async Task<DailyBudgetSummaryDto> ExecuteAsync(Guid budgetCycleId)
        {
            var cycle = await _cycleRepository.GetByIdAsync(budgetCycleId)
                ?? throw new InvalidOperationException("Budget cycle not found.");

            return new DailyBudgetSummaryDto
            {
                BudgetCycleId = cycle.Id,
                FundingSourceName = cycle.Source.Name,
                DailyCapacity = cycle.DailyCapacity.Amount,
                RemainingCapacity = cycle.RemainingCapacity.Amount,
                RemainingDays = cycle.RemainingDays
            };
        }
    }

}
