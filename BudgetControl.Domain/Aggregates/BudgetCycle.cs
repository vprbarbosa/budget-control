using BudgetControl.Domain.Categories;
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

        private BudgetCycle(
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

        public static BudgetCycle Create(
            FundingSource source,
            DateOnly startDate,
            int estimatedDurationInDays,
            decimal totalCapacity)
        {
            var period = new CyclePeriod(startDate, estimatedDurationInDays);
            var capacity = new Money(totalCapacity);

            return new BudgetCycle(source, period, capacity);
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

        // ===== Queries =====

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

        // ===== Commands (intention revealing) =====

        public void RegisterExpense(
            DateOnly date,
            decimal amount,
            SpendingCategory category,
            Guid userId,
            string description = "")
        {
            RegisterExpense(date, new Money(amount), category, userId, description);
        }

        private void RegisterExpense(
            DateOnly date,
            Money amount,
            SpendingCategory category,
            Guid userId,
            string description = "")
        {
            var day = _days.Single(d => d.Date == date);

            var expense = PartialExpense.Create(
                amount,
                category,
                userId,
                description);

            day.AddExpense(expense);
        }

        public void CloseDay(DateOnly date)
        {
            var day = _days.Single(d => d.Date == date);
            day.Close();
        }

        public void AdjustTotalCapacity(decimal newCapacity)
        {
            AdjustTotalCapacity(new Money(newCapacity));
        }

        private void AdjustTotalCapacity(Money newCapacity)
        {
            TotalCapacity = newCapacity;
        }

        public void AdjustPeriod(DateOnly startDate, int estimatedDurationInDays)
        {
            AdjustPeriod(new CyclePeriod(startDate, estimatedDurationInDays));
        }

        private void AdjustPeriod(CyclePeriod newPeriod)
        {
            Period = newPeriod;
            InitializeDays();
        }
    }
}
