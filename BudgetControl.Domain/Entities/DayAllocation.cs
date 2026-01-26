using BudgetControl.Domain.Common;
using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Domain.Entities
{
    public sealed class DayAllocation : Entity
    {
        public DateOnly Date { get; }
        
        private readonly List<PartialExpense> _expenses = new();
        public IReadOnlyCollection<PartialExpense> Expenses => _expenses;

        internal DayAllocation(DateOnly date, Guid? id = null)
            : base(id)
        {
            Date = date;
        }

        internal void AddExpense(PartialExpense expense)
        {
            _expenses.Add(expense);
        }

        public Money TotalSpent => _expenses.Aggregate(Money.Zero, (acc, e) => acc.Add(e.Amount));

        public bool IsClosed(DateOnly today) => Date < today;

        private DayAllocation()
        : base(null)
        {
        }
    }
}
