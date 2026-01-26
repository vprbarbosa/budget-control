using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Domain.Tests.Aggregates
{
    public sealed class BudgetCycleTests
    {
        [Fact]
        public void DailyCapacity_ShouldBeTotalCapacityDividedByRemainingDays_WhenNothingSpentAndNoDaysClosed()
        {
            // Arrange
            var cycle = TestData.Cycle(
                start: new DateOnly(2025, 1, 1),
                days: 30,
                capacity: 3000m);

            // Act
            var today = new DateOnly(2025, 1, 1);
            var daily = cycle.DailyCapacity(today);


            // Assert
            Assert.Equal(100m, daily.Amount);
        }

        [Fact]
        public void DailyCapacity_ShouldDecrease_WhenExpensesAreRegistered()
        {
            var today = new DateOnly(2025, 1, 1);

            // Arrange
            var cycle = TestData.Cycle(
                start: new DateOnly(2025, 1, 1),
                days: 30,
                capacity: 3000m);

            // meta inicial 100/dia
            // gastar 300 => restante 2700 => 2700/30 = 90
            cycle.RegisterExpense(300, "Combustível", today);

            // Act
            var daily = cycle.DailyCapacity(today);

            // Assert
            Assert.Equal(90m, daily.Amount);
        }

        [Fact]
        public void BigExpense_ShouldProjectNeedToHoldSpending_ForFutureDays()
        {
            var today = new DateOnly(2025, 1, 1);

            // Arrange
            // Exemplo: 10 dias, capacidade 500 => meta inicial 50
            var cycle = TestData.Cycle(
                start: new DateOnly(2025, 1, 1),
                days: 10,
                capacity: 500m);

            // gasto grande: 150 (3x a meta diária inicial)
            cycle.RegisterExpense(150, "Abastecer carro", today);

            // Act
            // restante 350, dias restantes 10 (nenhum fechado) => 35/dia
            var daily = cycle.DailyCapacity(today);

            // Assert
            Assert.Equal(35m, daily.Amount);

            // Interpretação (do domínio): 150 gasto num dia implicaria "segurar" alguns dias.
            // Aqui o teste prova a consequência matemática correta.
        }

        [Fact]
        public void AdjustTotalCapacity_ShouldRecalculateDailyCapacity()
        {
            var today = new DateOnly(2025, 1, 1);

            // Arrange
            // 10 dias, 500 => 50/dia
            var cycle = TestData.Cycle(
                start: new DateOnly(2025, 1, 1),
                days: 10,
                capacity: 500m);

            // Act
            cycle.AdjustTotalCapacity(600); // => 60/dia
            var daily = cycle.DailyCapacity(today);

            // Assert
            Assert.Equal(60m, daily.Amount);
        }

        [Fact]
        public void AdjustPeriod_ShouldRecreateDays_ToMatchNewDuration()
        {
            // Arrange
            var cycle = TestData.Cycle(
                start: new DateOnly(2025, 1, 1),
                days: 10,
                capacity: 500m);

            // Act
            cycle.AdjustPeriod(new DateOnly(2025, 1, 1), 30);

            // Assert
            Assert.Equal(30, cycle.Days.Count);
            Assert.Equal(new DateOnly(2025, 1, 30), cycle.Period.EstimatedEndDate);
        }
    }
}
