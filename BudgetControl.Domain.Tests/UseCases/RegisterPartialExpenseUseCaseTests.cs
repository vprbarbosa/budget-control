using BudgetControl.Application.DTOs;
using BudgetControl.Application.Infrastructure.InMemory;
using BudgetControl.Application.UseCases.RegisterPartialExpense;
using BudgetControl.Domain.Aggregates;
using BudgetControl.Domain.Categories;
using BudgetControl.Domain.Entities;

namespace BudgetControl.Domain.Tests.UseCases
{
    public sealed class RegisterPartialExpenseUseCaseTests
    {
        [Fact]
        public async Task Should_register_partial_expense_and_recalculate_daily_capacity()
        {
            // Arrange
            var today = new DateOnly(2025, 1, 1);

            var cycleRepo = new InMemoryBudgetCycleRepository();
            var categoryRepo = new InMemorySpendingCategoryRepository();

            var source = FundingSource.Create("Cartão");
            var category = SpendingCategory.Create("Combustível");
            categoryRepo.Add(category);

            var cycle = BudgetCycle.Create(
                source,
                startDate: today,
                estimatedDurationInDays: 10,
                totalCapacity: 500m
            );

            await cycleRepo.SaveAsync(cycle);

            var clock = new FakeClock(new DateOnly(2025, 1, 1));

            var useCase = new RegisterPartialExpenseUseCase(
                cycleRepo,
                clock
            );

            var input = new RegisterPartialExpenseInput
            {
                BudgetCycleId = cycle.Id,
                Amount = 150m,
                Description = "Abastecimento"
            };

            // Act
            await useCase.ExecuteAsync(input);

            // Assert
            var updated = (await cycleRepo.GetByIdAsync(cycle.Id))!;

            Assert.Equal(350m, updated.RemainingCapacity.Amount);

            var daily = updated.DailyCapacity(today);
            Assert.Equal(35m, daily.Amount); // 350 / 10
        }
    }
}
