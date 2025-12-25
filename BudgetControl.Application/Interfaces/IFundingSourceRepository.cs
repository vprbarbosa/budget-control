using BudgetControl.Domain.Entities;

namespace BudgetControl.Application.Interfaces
{
    public interface IFundingSourceRepository
    {
        Task<FundingSource?> GetByIdAsync(Guid id);
    }
}
