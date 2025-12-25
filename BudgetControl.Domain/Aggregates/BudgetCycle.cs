using BudgetControl.Domain.Common;
using BudgetControl.Domain.Entities;
using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Domain.Aggregates
{
    public sealed class BudgetCycle : Entity
    {
        public FundingSource Source { get; }
        public CyclePeriod Period { get; private set; }
        public Money TotalCapacity { get; private set; }

        private readonly List<DayAllocation> _days = new();
        public IReadOnlyCollection<DayAllocation> Days => _days;

        public BudgetCycle(
            FundingSource source,
            CyclePeriod period,
            Money totalCapacity,
            Guid? id = null)
            : base(id)
        {
            Source = source;
            Period = period;
            TotalCapacity = totalCapacity;

            InitializeDays();
        }

        private void InitializeDays()
        {
            _days.Clear();

            for (int i = 0; i < Period.EstimatedDurationInDays; i++)
            {
                _days.Add(new DayAllocation(
                    Period.StartDate.AddDays(i)));
            }
        }

        public Money TotalSpent =>
            _days.Aggregate(Money.Zero, (acc, d) => acc.Add(d.TotalSpent));

        public Money RemainingCapacity =>
            TotalCapacity.Subtract(TotalSpent);

        public int RemainingDays =>
            _days.Count(d => !d.IsClosed);

        public Money DailyCapacity =>
            RemainingDays == 0
                ? Money.Zero
                : new Money(RemainingCapacity.Amount / RemainingDays);

        public void AdjustTotalCapacity(Money newCapacity)
        {
            TotalCapacity = newCapacity;
        }

        public void AdjustPeriod(CyclePeriod newPeriod)
        {
            Period = newPeriod;
            InitializeDays();
        }
    }
}
