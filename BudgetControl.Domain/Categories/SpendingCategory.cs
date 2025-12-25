namespace BudgetControl.Domain.Categories
{
    public sealed class SpendingCategory
    {
        public Guid Id { get; }
        public string Name { get; }

        public SpendingCategory(Guid id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name is required.");

            Id = id;
            Name = name;
        }
    }
}
