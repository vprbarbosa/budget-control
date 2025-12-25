using BudgetControl.Domain.Common;

namespace BudgetControl.Domain.ValueObjects
{
    public sealed class CyclePeriod : ValueObject
    {
        public DateOnly StartDate { get; }
        public int EstimatedDurationInDays { get; }

        internal CyclePeriod(DateOnly startDate, int estimatedDurationInDays)
        {
            if (estimatedDurationInDays <= 0)
                throw new ArgumentOutOfRangeException(nameof(estimatedDurationInDays));

            StartDate = startDate;
            EstimatedDurationInDays = estimatedDurationInDays;
        }

        public DateOnly EstimatedEndDate =>
            StartDate.AddDays(EstimatedDurationInDays - 1);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return StartDate;
            yield return EstimatedDurationInDays;
        }
    }
}
