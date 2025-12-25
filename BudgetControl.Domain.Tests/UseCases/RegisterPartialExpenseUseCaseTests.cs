using BudgetControl.Application.DTOs;
using BudgetControl.Application.Infrastructure.InMemory;
using BudgetControl.Application.UseCases.RegisterPartialExpense;
using BudgetControl.Domain.Aggregates;
using BudgetControl.Domain.Categories;
using BudgetControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Domain.Tests.UseCases
{
    public sealed class RegisterPartialExpenseUseCaseTests
    {
        [Fact]
        public async Task Should_register_partial_expense_and_recalculate_daily_capacity()
        {
            // Arrange
            var cycleRepo = new InMemoryBudgetCycleRepository();
            var categoryRepo = new InMemorySpendingCategoryRepository();

            var source = new FundingSource("Cartão");
            var category = new SpendingCategory("Combustível");
            categoryRepo.Add(category);

            var cycle = BudgetCycle.Create(source, new DateOnly(2025, 1, 1), 10, 500m);
            await cycleRepo.SaveAsync(cycle);

            var useCase = new RegisterPartialExpenseUseCase(cycleRepo, categoryRepo);

            var input = new RegisterPartialExpenseInput
            {
                BudgetCycleId = cycle.Id,
                Date = new DateOnly(2025, 1, 1),
                Amount = 150m,
                SpendingCategoryId = category.Id,
                UserId = Guid.NewGuid(),
                Description = "Abastecimento"
            };

            // Act
            await useCase.ExecuteAsync(input);

            // Assert
            var updated = (await cycleRepo.GetByIdAsync(cycle.Id))!;
            Assert.Equal(350m, updated.RemainingCapacity.Amount);
            Assert.Equal(35m, updated.DailyCapacity.Amount); // 350 / 10
        }
    }
}
