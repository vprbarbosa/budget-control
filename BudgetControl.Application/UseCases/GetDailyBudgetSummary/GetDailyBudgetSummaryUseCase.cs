using BudgetControl.Application.Abstractions.Clock;
using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Application.DTOs;

namespace BudgetControl.Application.UseCases.GetDailyBudgetSummary
{
    public sealed class GetDailyBudgetSummaryUseCase
    {
        private readonly IBudgetCycleRepository _cycleRepository;
        private readonly IClock _clock;

        public GetDailyBudgetSummaryUseCase(
            IBudgetCycleRepository cycleRepository,
            IClock clock)
        {
            _cycleRepository = cycleRepository;
            _clock = clock;
        }

        public async Task<IReadOnlyCollection<DailyBudgetSummaryDto>> ExecuteAsync()
        {
            var today = _clock.Today();

            var cycles = await _cycleRepository.GetActiveCyclesAsync(today);

            return cycles.Select(cycle => new DailyBudgetSummaryDto
            {
                BudgetCycleId = cycle.Id,
                FundingSourceName = cycle.Source.Name,
                DailyCapacity = cycle.DailyCapacity.Amount,
                RemainingCapacity = cycle.RemainingCapacity.Amount,
                RemainingDays = cycle.RemainingDays
            }).ToList();
        }
    }
}
