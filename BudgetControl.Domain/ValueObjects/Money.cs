using BudgetControl.Domain.Common;

namespace BudgetControl.Domain.ValueObjects
{
    public sealed class Money : ValueObject
    {
        public decimal Amount { get; }

        internal Money(decimal amount)
        {
            Amount = amount;
        }

        public static Money Zero => new(0);

        public Money Abs()
        {
            return Amount < 0
                ? new Money(-Amount)
                : this;
        }

        public static Money FromDecimal(decimal amount)
        {
            if (amount < 0)
                throw new InvalidOperationException("Valor monetário não pode ser negativo.");

            return new Money(amount);
        }

        // ⚠️ ESTES DEVEM SER PUBLIC
        public Money Add(Money other) => new(Amount + other.Amount);

        public Money Subtract(Money other)
        {
            return new Money(Amount - other.Amount);
        }

        public bool IsLessThan(Money other) => Amount < other.Amount;

        public bool IsGreaterThan(Money other) => Amount > other.Amount;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Amount;
        }
    }
}
