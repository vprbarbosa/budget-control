using BudgetControl.Domain.Aggregates;

namespace BudgetControl.Application.Abstractions.Persistence
{
    public interface IBudgetCycleRepository
    {
        Task<IReadOnlyCollection<BudgetCycle>> GetActiveCyclesAsync(DateOnly date);

        Task<BudgetCycle?> GetByIdAsync(Guid id);

        Task SaveAsync(BudgetCycle cycle);

        IReadOnlyCollection<BudgetCycle> All();
    }
}
