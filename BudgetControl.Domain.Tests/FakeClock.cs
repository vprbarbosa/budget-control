using BudgetControl.Application.Abstractions.Clock;

namespace BudgetControl.Domain.Tests
{
    public sealed class FakeClock : IClock
    {
        private readonly DateOnly _today;
        public FakeClock(DateOnly today) => _today = today;
        public DateOnly Today() => _today;
    }
}
