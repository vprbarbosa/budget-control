using BudgetControl.Domain.Aggregates;

namespace BudgetControl.Application.DTOs
{

    public sealed class BudgetCycleCreatedDto
    {
        public Guid Id { get; init; }
        public DateOnly StartDate { get; init; }
        public int EstimatedDurationInDays { get; init; }
        public decimal TotalCapacity { get; init; }

        public static BudgetCycleCreatedDto From(BudgetCycle cycle)
        {
            return new BudgetCycleCreatedDto
            {
                Id = cycle.Id,
                StartDate = cycle.Period.StartDate,
                EstimatedDurationInDays = cycle.Period.EstimatedDurationInDays,
                TotalCapacity = cycle.TotalCapacity.Amount
            };
        }
    }
}
