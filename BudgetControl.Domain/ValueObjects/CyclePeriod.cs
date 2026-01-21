using BudgetControl.Domain.Common;
using System.Text.Json.Serialization;

namespace BudgetControl.Domain.ValueObjects
{
    public sealed class CyclePeriod : ValueObject
    {
        [JsonInclude]
        public DateOnly StartDate { get; private set; }

        [JsonInclude]
        public int EstimatedDurationInDays { get; private set; }

        internal CyclePeriod(DateOnly startDate, int estimatedDurationInDays)
        {
            if (estimatedDurationInDays <= 0)
                throw new ArgumentOutOfRangeException(nameof(estimatedDurationInDays));

            StartDate = startDate;
            EstimatedDurationInDays = estimatedDurationInDays;
        }

        [JsonConstructor]
        private CyclePeriod()
        {
            // Snapshot / EF rehydration only
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
