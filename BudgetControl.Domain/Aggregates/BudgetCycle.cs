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
        public DateOnly? EndDate { get; private set; }


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

        public void EnsureDaysUpTo(DateOnly endDate)
        {
            if (_days.Count == 0)
                throw new InvalidOperationException("Cycle has no initialized days.");

            var lastExistingDate = _days.Max(d => d.Date);

            if (endDate <= lastExistingDate)
                return;

            var daysToAdd =
                endDate.DayNumber - lastExistingDate.DayNumber;

            for (int i = 1; i <= daysToAdd; i++)
            {
                _days.Add(new DayAllocation(
                    lastExistingDate.AddDays(i)));
            }
        }

        public void AdjustTotalCapacity(Money newTotalCapacity)
        {
            if (newTotalCapacity.IsLessThan(TotalSpent))
                throw new InvalidOperationException(
                    "O orçamento total não pode ser menor que o valor já gasto."
                );

            TotalCapacity = newTotalCapacity;
        }

        // ===== Queries =====

        public Money TotalSpent =>
            ActiveDays.Aggregate(Money.Zero, (acc, d) => acc.Add(d.TotalSpent));

        public Money RemainingCapacity =>
            TotalCapacity.Subtract(TotalSpent);

        public int RemainingDays(DateOnly today) =>
            ActiveDays.Count(d => !d.IsClosed(today));


        public Money DailyCapacity(DateOnly today)
        {
            var remainingDays = RemainingDays(today);

            if (remainingDays == 0)
                return Money.Zero;

            if (RemainingCapacity.IsLessThan(Money.Zero))
                return Money.Zero;

            return Money.FromDecimal(
                RemainingCapacity.Amount / remainingDays
            );
        }

        private IEnumerable<DayAllocation> ActiveDays =>
            _days.Where(d =>
                d.Date >= Period.StartDate &&
                (EndDate == null || d.Date <= EndDate.Value));


        private DayAllocation? CurrentDay(DateOnly today) =>
            ActiveDays
                .OrderBy(d => d.Date)
                .FirstOrDefault(d => !d.IsClosed(today));

        // ===== Commands =====

        public void RegisterExpense(
            decimal amount,
            string description,
            DateOnly today)
        {
            var day = ActiveDays
                .Where(d => d.Date <= today)
                .OrderBy(d => d.Date)
                .LastOrDefault(d => !d.IsClosed(today));

            if (day is null)
                throw new InvalidOperationException("No open day available in this cycle.");

            var moneyAmount = Money.FromDecimal(amount);

            var expense = PartialExpense.Create(
                moneyAmount,
                description);

            day.AddExpense(expense);
        }

        public bool IsOverBudget => TotalSpent.IsGreaterThan(TotalCapacity);

        public void AdjustTotalCapacity(decimal newCapacity)
        {
            AdjustTotalCapacity(new Money(newCapacity));
        }

        public void AdjustPeriod(DateOnly startDate, int estimatedDurationInDays)
        {
            Period = new CyclePeriod(startDate, estimatedDurationInDays);
            InitializeDays();
        }

        public void DefineEndDate(DateOnly endDate, DateOnly today)
        {
            if (endDate < Period.StartDate)
                throw new InvalidOperationException("Data final não pode ser anterior à data inicial.");

            var lastClosedDay = _days
                .Where(d => d.IsClosed(today))
                .OrderByDescending(d => d.Date)
                .FirstOrDefault();

            if (lastClosedDay is not null && endDate < lastClosedDay.Date)
                throw new InvalidOperationException(
                    "Data final não pode ser anterior ao último dia já fechado.");

            EndDate = endDate;
            EnsureDaysUpTo(endDate);
        }

        private BudgetCycle() : base(null)
        {
            // EF Core only
        }
    }

}
