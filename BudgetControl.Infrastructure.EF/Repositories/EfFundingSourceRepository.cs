using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Domain.Entities;
using BudgetControl.Infrastructure.EF.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetControl.Infrastructure.EF.Repositories
{
    public sealed class EfFundingSourceRepository : IFundingSourceRepository
    {
        private readonly BudgetControlDbContext _db;

        public EfFundingSourceRepository(BudgetControlDbContext db)
        {
            _db = db;
        }

        public Task<FundingSource?> GetByIdAsync(Guid id)
            => _db.FundingSources.FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(FundingSource source)
        {
            await _db.FundingSources.AddAsync(source);
            await _db.SaveChangesAsync();
        }
    }
}
