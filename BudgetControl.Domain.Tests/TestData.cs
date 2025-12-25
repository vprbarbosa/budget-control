using BudgetControl.Domain.Aggregates;
using BudgetControl.Domain.Categories;
using BudgetControl.Domain.Entities;
using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Domain.Tests
{
    internal static class TestData
    {
        public static FundingSource Card(string name = "Cartão de crédito")
            => FundingSource.Create(name);

        public static SpendingCategory Category(string name = "Estacionamento")
            => SpendingCategory.Create(name);

        public static BudgetCycle Cycle(
            DateOnly start,
            int days,
            decimal capacity,
            FundingSource? source = null)
        {
            source ??= Card();

            return BudgetCycle.Create(
                source,
                start,
                days,
                capacity);
        }
    }
}
