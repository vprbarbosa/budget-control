using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Pwa.Infrastructure.Application
{
    public interface IBudgetControlApp
    {
        Task<Guid> CreateCycleAsync(
            string fundingSourceName,
            DateOnly startDate,
            int estimatedDays,
            decimal totalCapacity,
            CancellationToken ct = default);

        Task RegisterExpenseAsync(
            Guid cycleId,
            decimal amount,
            string description,
            CancellationToken ct = default);
    }

}
