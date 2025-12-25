using BudgetControl.Application.Abstractions.Clock;

namespace BudgetControl.Application.Infrastructure.Clock
{
    public sealed class SystemClock : IClock
    {
        public DateOnly Today()
            => DateOnly.FromDateTime(DateTime.Today);
    }
}
