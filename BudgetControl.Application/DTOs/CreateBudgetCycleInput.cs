namespace BudgetControl.Application.DTOs
{
    public sealed class CreateBudgetCycleInput
    {
        public DateOnly StartDate { get; init; }
        public int EstimatedDurationInDays { get; init; }
        public decimal TotalCapacity { get; init; }

        // temporário, até termos gestão de fontes
        public Guid FundingSourceId { get; init; }
    }
}
