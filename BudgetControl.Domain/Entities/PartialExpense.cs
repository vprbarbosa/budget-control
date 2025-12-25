using BudgetControl.Domain.Categories;
using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Domain.Entities
{
    public sealed class PartialExpense
    {
        public Guid Id { get; }
        public Money Amount { get; }
        public string Description { get; }
        public SpendingCategory Category { get; }
        public DateTime CreatedAt { get; }
        public Guid CreatedBy { get; }

        public PartialExpense(
            Guid id,
            Money amount,
            SpendingCategory category,
            Guid createdBy,
            string description = "")
        {
            Id = id;
            Amount = amount;
            Category = category;
            CreatedBy = createdBy;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
