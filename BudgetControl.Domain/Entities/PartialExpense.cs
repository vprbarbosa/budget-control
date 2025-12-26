using BudgetControl.Domain.Categories;
using BudgetControl.Domain.Common;
using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Domain.Entities
{
    public sealed class PartialExpense : Entity
    {
        public Money Amount { get; }
        public SpendingCategory Category { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }
        public Guid CreatedBy { get; }

        private PartialExpense(
            Money amount,
            SpendingCategory category,
            Guid createdBy,
            string description,
            Guid? id = null)
            : base(id)
        {
            Amount = amount;
            Category = category;
            CreatedBy = createdBy;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }

        public static PartialExpense Create(
            Money amount,
            SpendingCategory category,
            Guid createdBy,
            string description = "")
        {
            return new PartialExpense(amount, category, createdBy, description);
        }

        private PartialExpense()
        : base(null)
        {
        }
    }
}
