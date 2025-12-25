using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Domain.Categories;

namespace BudgetControl.Application.Infrastructure.InMemory
{
    public sealed class InMemorySpendingCategoryRepository : ISpendingCategoryRepository
    {
        private readonly Dictionary<Guid, SpendingCategory> _categories = new();

        public void Add(SpendingCategory category)
        {
            _categories[category.Id] = category;
        }

        public Task<SpendingCategory?> GetByIdAsync(Guid id)
        {
            _categories.TryGetValue(id, out var category);
            return Task.FromResult(category);
        }
    }
}
