using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Domain.Tests.ValueObjects
{
    public sealed class MoneyTests
    {
        [Fact]
        public void Constructor_ShouldThrow_WhenAmountIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Money(-1));
        }

        [Fact]
        public void Zero_ShouldBeZero()
        {
            Assert.Equal(0m, Money.Zero.Amount);
        }

        [Fact]
        public void Add_ShouldSumAmounts()
        {
            var a = new Money(10);
            var b = new Money(25);

            var result = a.Add(b);

            Assert.Equal(35m, result.Amount);
        }

        [Fact]
        public void Subtract_ShouldThrow_WhenInsufficientFunds()
        {
            var a = new Money(10);
            var b = new Money(11);

            Assert.Throws<InvalidOperationException>(() => a.Subtract(b));
        }

        [Fact]
        public void Subtract_ShouldReturnDifference()
        {
            var a = new Money(50);
            var b = new Money(20);

            var result = a.Subtract(b);

            Assert.Equal(30m, result.Amount);
        }
    }
}
