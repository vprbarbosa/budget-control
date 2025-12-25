using BudgetControl.Domain.Aggregates;

namespace BudgetControl.Application.Interfaces
{
    public interface IBudgetCycleRepository
    {
        Task AddAsync(BudgetCycle cycle);
        Task SaveChangesAsync();
    }
}
