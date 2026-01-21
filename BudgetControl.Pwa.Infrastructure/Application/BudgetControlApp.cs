using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Pwa.Infrastructure.Application
{
    public sealed class BudgetControlApp : IBudgetControlApp
    {
        private readonly CreateBudgetCycleUseCase _createCycle;
        private readonly RegisterExpenseUseCase _registerExpense;
        private readonly CloseDayUseCase _closeDay;

        public BudgetControlApp(
            CreateBudgetCycleUseCase createCycle,
            RegisterExpenseUseCase registerExpense,
            CloseDayUseCase closeDay)
        {
            _createCycle = createCycle;
            _registerExpense = registerExpense;
            _closeDay = closeDay;
        }

        public Task<Guid> CreateCycleAsync(
            string fundingSourceName,
            DateOnly startDate,
            int estimatedDays,
            decimal totalCapacity,
            CancellationToken ct = default)
        {
            return _createCycle.ExecuteAsync(
                fundingSourceName,
                startDate,
                estimatedDays,
                totalCapacity,
                ct);
        }

        public Task RegisterExpenseAsync(
            Guid cycleId,
            decimal amount,
            string description,
            CancellationToken ct = default)
        {
            return _registerExpense.ExecuteAsync(
                cycleId,
                amount,
                description,
                ct);
        }

        public Task CloseCurrentDayAsync(
            Guid cycleId,
            CancellationToken ct = default)
        {
            return _closeDay.ExecuteAsync(cycleId, ct);
        }
    }
}
