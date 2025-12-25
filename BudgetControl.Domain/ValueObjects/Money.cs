using BudgetControl.Domain.Common;

namespace BudgetControl.Domain.ValueObjects
{
    public sealed class Money : ValueObject
    {
        public decimal Amount { get; }

        public Money(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            Amount = amount;
        }

        public static Money Zero => new(0);

        public Money Add(Money other) => new(Amount + other.Amount);

        public Money Subtract(Money other)
        {
            if (other.Amount > Amount)
                throw new InvalidOperationException("Insufficient funds.");

            return new Money(Amount - other.Amount);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Amount;
        }
    }
}
