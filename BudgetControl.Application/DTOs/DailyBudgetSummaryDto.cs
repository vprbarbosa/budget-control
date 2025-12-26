namespace BudgetControl.Application.DTOs
{
    public sealed class DailyBudgetSummaryDto
    {
        public Guid BudgetCycleId { get; init; }
        public string BudgetCycleName { get; init; }
        public string FundingSourceName { get; init; } = string.Empty;
        public decimal DailyCapacity { get; init; }
        public decimal RemainingCapacity { get; init; }
        public int RemainingDays { get; init; }
    }
}
