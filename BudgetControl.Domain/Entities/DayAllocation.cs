using BudgetControl.Domain.Common;
using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Domain.Entities
{
    public sealed class DayAllocation : Entity
    {
        public DateOnly Date { get; }
        public bool IsClosed { get; private set; }

        private readonly List<PartialExpense> _expenses = new();
        public IReadOnlyCollection<PartialExpense> Expenses => _expenses;

        internal DayAllocation(DateOnly date, Guid? id = null)
            : base(id)
        {
            Date = date;
        }

        internal void AddExpense(PartialExpense expense)
        {
            if (IsClosed)
                throw new InvalidOperationException("Cannot add expense to a closed day.");

            _expenses.Add(expense);
        }

        internal void Close()
        {
            IsClosed = true;
        }

        public Money TotalSpent =>
            _expenses.Aggregate(Money.Zero, (acc, e) => acc.Add(e.Amount));

        private DayAllocation()
        : base(null)
        {
        }
    }
}
