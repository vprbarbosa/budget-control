namespace BudgetControl.Domain.ValueObjects
{
    public sealed class CyclePeriod
    {
        public DateOnly StartDate { get; }
        public int EstimatedDurationInDays { get; }

        public CyclePeriod(DateOnly startDate, int estimatedDurationInDays)
        {
            if (estimatedDurationInDays <= 0)
                throw new ArgumentOutOfRangeException(nameof(estimatedDurationInDays));

            StartDate = startDate;
            EstimatedDurationInDays = estimatedDurationInDays;
        }

        public DateOnly EstimatedEndDate =>
            StartDate.AddDays(EstimatedDurationInDays - 1);
    }
}
