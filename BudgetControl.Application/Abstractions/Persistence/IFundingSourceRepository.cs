using BudgetControl.Domain.Entities;

namespace BudgetControl.Application.Abstractions.Persistence
{
    public interface IFundingSourceRepository
    {
        Task AddAsync(FundingSource source);

        Task<FundingSource?> GetByIdAsync(Guid id);
    }
}
