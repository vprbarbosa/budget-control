using BudgetControl.Domain.Categories;

namespace BudgetControl.Application.Abstractions.Persistence
{
    public interface ISpendingCategoryRepository
    {
        Task<SpendingCategory?> GetByIdAsync(Guid id);
    }
}
