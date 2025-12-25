using BudgetControl.Domain.Entities;

namespace BudgetControl.Application.Abstractions.Persistence
{
    public interface IFundingSourceRepository
    {
        void Add(FundingSource source);

        Task<FundingSource?> GetByIdAsync(Guid id);
    }
}
