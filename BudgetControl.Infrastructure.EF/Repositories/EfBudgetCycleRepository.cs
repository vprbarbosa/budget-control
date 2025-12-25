using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Domain.Aggregates;
using BudgetControl.Infrastructure.EF.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetControl.Infrastructure.EF.Repositories
{
    public sealed class EfBudgetCycleRepository : IBudgetCycleRepository
    {
        private readonly BudgetControlDbContext _db;

        public EfBudgetCycleRepository(BudgetControlDbContext db)
        {
            _db = db;
        }

        public async Task<BudgetCycle?> GetByIdAsync(Guid id)
        {
            return await _db.BudgetCycles
                .Include(x => x.Source)
                .Include(x => x.Days)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyCollection<BudgetCycle>> GetActiveCyclesAsync(DateOnly date)
        {
            return await _db.BudgetCycles
                .Include(x => x.Source)
                .Where(c => c.Period.StartDate <= date && c.Period.EstimatedEndDate >= date)
                .ToListAsync();
        }

        public async Task SaveAsync(BudgetCycle cycle)
        {
            var exists = await _db.BudgetCycles.AnyAsync(x => x.Id == cycle.Id);

            if (!exists)
                await _db.BudgetCycles.AddAsync(cycle);

            await _db.SaveChangesAsync();
        }
    }
}
