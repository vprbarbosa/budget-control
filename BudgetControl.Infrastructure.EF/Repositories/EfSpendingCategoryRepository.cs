using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Domain.Categories;
using BudgetControl.Infrastructure.EF.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetControl.Infrastructure.EF.Repositories
{
    public sealed class EfSpendingCategoryRepository : ISpendingCategoryRepository
    {
        private readonly BudgetControlDbContext _db;

        public EfSpendingCategoryRepository(BudgetControlDbContext db)
        {
            _db = db;
        }

        public Task<SpendingCategory?> GetByIdAsync(Guid id)
            => _db.SpendingCategories.FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(SpendingCategory category)
        {
            await _db.SpendingCategories.AddAsync(category);
            await _db.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<SpendingCategory>> ListAsync()
            => await _db.SpendingCategories.OrderBy(x => x.Name).ToListAsync();
    }
}
