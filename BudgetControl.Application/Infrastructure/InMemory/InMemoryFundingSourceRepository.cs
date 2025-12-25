using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Domain.Entities;

namespace BudgetControl.Application.Infrastructure.InMemory
{
    public sealed class InMemoryFundingSourceRepository : IFundingSourceRepository
    {
        private readonly Dictionary<Guid, FundingSource> _sources = new();

        public Task AddAsync(FundingSource source)
            => Task.FromResult(_sources[source.Id] = source);

        public Task<FundingSource?> GetByIdAsync(Guid id)
        {
            _sources.TryGetValue(id, out var source);
            return Task.FromResult(source);
        }
    }
}
