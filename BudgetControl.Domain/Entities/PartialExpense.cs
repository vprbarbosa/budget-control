using BudgetControl.Domain.Categories;
using BudgetControl.Domain.Common;
using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Domain.Entities
{
    public sealed class PartialExpense : Entity
    {
        public Money Amount { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }
        
        private PartialExpense(
            Money amount,
            string description,
            Guid? id = null)
            : base(id)
        {
            Amount = amount;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }

        public static PartialExpense Create(
            Money amount,
            string description = "")
        {
            return new PartialExpense(amount, description);
        }

        private PartialExpense()
        : base(null)
        {
        }
    }
}
