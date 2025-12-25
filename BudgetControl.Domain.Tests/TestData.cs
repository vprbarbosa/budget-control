using BudgetControl.Domain.Aggregates;
using BudgetControl.Domain.Categories;
using BudgetControl.Domain.Entities;
using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Domain.Tests
{
    internal static class TestData
    {
        public static FundingSource Card(string name = "Cartão de crédito")
            => new FundingSource(name);

        public static SpendingCategory Category(string name = "Estacionamento")
            => new SpendingCategory(name);

        public static BudgetCycle Cycle(
            DateOnly start,
            int days,
            decimal capacity,
            FundingSource? source = null)
        {
            source ??= Card();
            var period = new CyclePeriod(start, days);
            return BudgetCycle.Create(source, period, new Money(capacity));
        }
    }
}
