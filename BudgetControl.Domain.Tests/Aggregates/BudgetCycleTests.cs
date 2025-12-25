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
            var daily = cycle.DailyCapacity;

            // Assert
            Assert.Equal(100m, daily.Amount);
        }

        [Fact]
        public void DailyCapacity_ShouldIncrease_WhenSomeDaysAreClosed()
        {
            // Arrange
            var cycle = TestData.Cycle(
                start: new DateOnly(2025, 1, 1),
                days: 30,
                capacity: 3000m);

            // Fechar 10 dias sem gastar => denominador cai de 30 para 20
            for (int i = 0; i < 10; i++)
                cycle.CloseDay(new DateOnly(2025, 1, 1).AddDays(i));

            // Act
            var daily = cycle.DailyCapacity;

            // Assert
            Assert.Equal(150m, daily.Amount);
        }

        [Fact]
        public void DailyCapacity_ShouldDecrease_WhenExpensesAreRegistered()
        {
            // Arrange
            var cycle = TestData.Cycle(
                start: new DateOnly(2025, 1, 1),
                days: 30,
                capacity: 3000m);

            var cat = TestData.Category();
            var userId = Guid.NewGuid();

            // meta inicial 100/dia
            // gastar 300 => restante 2700 => 2700/30 = 90
            cycle.RegisterExpense(300, cat, userId, "Combustível");

            // Act
            var daily = cycle.DailyCapacity;

            // Assert
            Assert.Equal(90m, daily.Amount);
        }

        [Fact]
        public void BigExpense_ShouldProjectNeedToHoldSpending_ForFutureDays()
        {
            // Arrange
            // Exemplo: 10 dias, capacidade 500 => meta inicial 50
            var cycle = TestData.Cycle(
                start: new DateOnly(2025, 1, 1),
                days: 10,
                capacity: 500m);

            var cat = TestData.Category();
            var userId = Guid.NewGuid();

            // gasto grande: 150 (3x a meta diária inicial)
            cycle.RegisterExpense(150, cat, userId, "Abastecer carro");

            // Act
            // restante 350, dias restantes 10 (nenhum fechado) => 35/dia
            var daily = cycle.DailyCapacity;

            // Assert
            Assert.Equal(35m, daily.Amount);

            // Interpretação (do domínio): 150 gasto num dia implicaria "segurar" alguns dias.
            // Aqui o teste prova a consequência matemática correta.
        }

        [Fact]
        public void ClosingDay_ShouldReduceDenominator_AndRecalculateDailyCapacity()
        {
            // Arrange
            // 10 dias, 500 => 50/dia
            var cycle = TestData.Cycle(
                start: new DateOnly(2025, 1, 1),
                days: 10,
                capacity: 500m);

            // Fecha 2 dias (sem gastar) => 8 dias restantes => 62.5/dia
            cycle.CloseDay(new DateOnly(2025, 1, 1));
            cycle.CloseDay(new DateOnly(2025, 1, 2));

            // Act
            var daily = cycle.DailyCapacity;

            // Assert
            Assert.Equal(62.5m, daily.Amount);
        }

        [Fact]
        public void RegisterExpense_ShouldThrow_WhenNoOpenDayAvailable()
        {
            // Arrange
            var cycle = TestData.Cycle(
                start: new DateOnly(2025, 1, 1),
                days: 1,
                capacity: 500m);

            cycle.CloseDay(new DateOnly(2025, 1, 1));

            var cat = TestData.Category();
            var userId = Guid.NewGuid();

            // Act + Assert
            Assert.Throws<InvalidOperationException>(() =>
                cycle.RegisterExpense(10, cat, userId, "Qualquer"));
        }


        [Fact]
        public void DailyCapacity_ShouldBeZero_WhenAllDaysAreClosed()
        {
            // Arrange
            var start = new DateOnly(2025, 1, 1);
            var cycle = TestData.Cycle(start, days: 3, capacity: 300m);

            cycle.CloseDay(start);
            cycle.CloseDay(start.AddDays(1));
            cycle.CloseDay(start.AddDays(2));

            // Act
            var daily = cycle.DailyCapacity;

            // Assert
            Assert.Equal(0m, daily.Amount);
        }

        [Fact]
        public void AdjustTotalCapacity_ShouldRecalculateDailyCapacity()
        {
            // Arrange
            // 10 dias, 500 => 50/dia
            var cycle = TestData.Cycle(
                start: new DateOnly(2025, 1, 1),
                days: 10,
                capacity: 500m);

            // Act
            cycle.AdjustTotalCapacity(600); // => 60/dia
            var daily = cycle.DailyCapacity;

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
