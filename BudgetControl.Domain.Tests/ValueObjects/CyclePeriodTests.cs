using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Domain.Tests.ValueObjects
{
    public sealed class CyclePeriodTests
    {
        [Fact]
        public void Constructor_ShouldThrow_WhenDaysIsZeroOrLess()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CyclePeriod(new DateOnly(2025, 1, 1), 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CyclePeriod(new DateOnly(2025, 1, 1), -10));
        }

        [Fact]
        public void EstimatedEndDate_ShouldBeStartPlusDaysMinusOne()
        {
            var period = new CyclePeriod(new DateOnly(2025, 1, 1), 30);

            Assert.Equal(new DateOnly(2025, 1, 30), period.EstimatedEndDate);
        }
    }
}
