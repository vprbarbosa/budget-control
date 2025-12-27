using BudgetControl.Domain.Entities;

namespace BudgetControl.Application.Abstractions.Persistence
{
    public interface IFundingSourceRepository
    {
        Task SaveAsync(FundingSource source);

        Task<FundingSource?> GetByIdAsync(Guid id);

        Task<IReadOnlyCollection<FundingSource>> GetAllAsync();
    }
}
