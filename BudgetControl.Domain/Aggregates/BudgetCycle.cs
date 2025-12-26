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

        private DayAllocation? CurrentDay =>
            _days
                .OrderBy(d => d.Date)
                .FirstOrDefault(d => !d.IsClosed);

        // ===== Commands =====

        public void RegisterExpense(
            decimal amount,            
            string description = "")
        {
            var day = CurrentDay
                ?? throw new InvalidOperationException("No open day available in this cycle.");

            var expense = PartialExpense.Create(
                new Money(amount),                
                description);

            day.AddExpense(expense);
        }

        public void CloseCurrentDay()
        {
            var day = CurrentDay
                ?? throw new InvalidOperationException("No open day to close.");

            day.Close();
        }

        // Admin / histórico
        public void CloseDay(DateOnly date)
        {
            var day = _days.Single(d => d.Date == date);

            if (day.IsClosed)
                throw new InvalidOperationException("Day already closed.");

            day.Close();
        }

        public void AdjustTotalCapacity(decimal newCapacity)
        {
            TotalCapacity = new Money(newCapacity);
        }

        public void AdjustPeriod(DateOnly startDate, int estimatedDurationInDays)
        {
            Period = new CyclePeriod(startDate, estimatedDurationInDays);
            InitializeDays();
        }

        private BudgetCycle() : base(null)
        {
            // EF Core only
        }
    }

}
