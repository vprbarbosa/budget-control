using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Domain.Aggregates;

namespace BudgetControl.Application.Infrastructure.InMemory
{
    public sealed class InMemoryBudgetCycleRepository : IBudgetCycleRepository
    {
        private readonly List<BudgetCycle> _cycles = new();

        public Task<IReadOnlyCollection<BudgetCycle>> GetActiveCyclesAsync(
            DateOnly date)
        {
            var result = _cycles
                .Where(c =>
                    c.Period.StartDate <= date &&
                    c.Period.EstimatedEndDate >= date)
                .ToList()
                .AsReadOnly();

            return Task.FromResult<IReadOnlyCollection<BudgetCycle>>(result);
        }

        public Task<BudgetCycle?> GetByIdAsync(
            Guid id)
        {
            var cycle = _cycles.SingleOrDefault(c => c.Id == id);
            return Task.FromResult(cycle);
        }

        public Task SaveAsync(
            BudgetCycle cycle)
        {
            if (_cycles.All(c => c.Id != cycle.Id))
                _cycles.Add(cycle);

            return Task.CompletedTask;
        }

        public IReadOnlyCollection<BudgetCycle> All() 
            => _cycles.AsReadOnly();

        public Task<IReadOnlyCollection<BudgetCycle>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
