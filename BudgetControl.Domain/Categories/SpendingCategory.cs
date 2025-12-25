using BudgetControl.Domain.Common;

namespace BudgetControl.Domain.Categories
{
    public sealed class SpendingCategory : Entity
    {
        public string Name { get; }

        public SpendingCategory(string name, Guid? id = null)
            : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name is required.");

            Name = name;
        }
    }
}
