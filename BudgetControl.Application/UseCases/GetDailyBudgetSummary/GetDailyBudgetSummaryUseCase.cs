using BudgetControl.Application.Abstractions.Clock;
using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Application.DTOs;
using BudgetControl.Domain.Aggregates;
using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Application.UseCases.GetDailyBudgetSummary
{
    public sealed class GetDailyBudgetSummaryUseCase
    {
        private readonly IBudgetCycleRepository _cycleRepository;
        private readonly IClock _clock;

        public GetDailyBudgetSummaryUseCase(IBudgetCycleRepository cycleRepository, IClock clock)
        {
            _cycleRepository = cycleRepository;
            _clock = clock;
        }

        public async Task<DailyBudgetSummaryDto> ExecuteAsync(Guid budgetCycleId)
        {
            var cycle = await _cycleRepository.GetByIdAsync(budgetCycleId)
                ?? throw new InvalidOperationException("Budget cycle not found.");

            var dailyTarget = CalculateDailyTarget(cycle, _clock.Today());

            return new DailyBudgetSummaryDto
            {
                BudgetCycleId = cycle.Id,
                FundingSourceName = cycle.Source.Name,
                DailyCapacity = dailyTarget.Amount,
                RemainingCapacity = cycle.RemainingCapacity.Amount,
                RemainingDays = cycle.RemainingDays
            };
        }

        private static Money CalculateDailyTarget(BudgetCycle cycle, DateOnly today)
        {
            var todayAllocation = cycle.Days.FirstOrDefault(d => d.Date == today);
            var spentToday = todayAllocation?.TotalSpent ?? Money.Zero;

            var baseCapacity =
                cycle.RemainingCapacity.Add(spentToday);

            if (cycle.RemainingDays == 0)
                return Money.Zero;

            return Money.FromDecimal(
                baseCapacity.Amount / cycle.RemainingDays
            );
        }
    }

}
