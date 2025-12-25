namespace BudgetControl.Application.DTOs
{
    public sealed class RegisterPartialExpenseInput
    {
        public Guid BudgetCycleId { get; init; }
        public decimal Amount { get; init; }
        public Guid SpendingCategoryId { get; init; }
        public Guid UserId { get; init; }
        public string Description { get; init; } = string.Empty;
    }
}
