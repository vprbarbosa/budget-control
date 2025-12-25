namespace BudgetControl.Application.DTOs
{
    public sealed class CreateBudgetCycleInput
    {
        public Guid FundingSourceId { get; init; }
        public DateOnly StartDate { get; init; }
        public int EstimatedDurationInDays { get; init; }
        public decimal TotalCapacity { get; init; }
    }
}
