using BudgetControl.Domain.Common;

namespace BudgetControl.Domain.Categories
{
    public sealed class SpendingCategory : Entity
    {
        public string Name { get; }

        private SpendingCategory(string name, Guid? id = null)
            : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name is required.", nameof(name));

            Name = name;
        }

        public static SpendingCategory Create(string name)
            => new SpendingCategory(name);

        public static SpendingCategory Default =>
            new SpendingCategory(
                "Geral",
                Guid.Parse("00000000-0000-0000-0000-000000000001"));

        private SpendingCategory()
        : base(null)
        {

        }
    }

}
