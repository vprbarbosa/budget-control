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

            var source = FundingSource.Create("Cartão");
            var category = SpendingCategory.Create("Combustível");
            categoryRepo.Add(category);

            var cycle = BudgetCycle.Create(source, new DateOnly(2025, 1, 1), 10, 500m);
            await cycleRepo.SaveAsync(cycle);

            var useCase = new RegisterPartialExpenseUseCase(cycleRepo, categoryRepo);

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
            Assert.Equal(35m, updated.DailyCapacity.Amount); // 350 / 10
        }

        [Fact]
        public void RegisterExpense_ShouldAddToFirstOpenDay()
        {
            var source = FundingSource.Create("Vale Refeição");
            var cycle = BudgetCycle.Create(
                source,
                DateOnly.FromDateTime(DateTime.Today),
                3,
                300);

            cycle.CloseDay(DateOnly.FromDateTime(DateTime.Today));

            cycle.RegisterExpense(
                50,
                "Almoço");

            var secondDay = cycle.Days.ElementAt(1);

            Assert.Equal(50, secondDay.TotalSpent.Amount);
        }

        [Fact]
        public void RegisterExpense_ShouldFail_WhenAllDaysClosed()
        {
            var source = FundingSource.Create("Cartão");
            var cycle = BudgetCycle.Create(
                source,
                DateOnly.FromDateTime(DateTime.Today),
                1,
                100);

            cycle.CloseDay(DateOnly.FromDateTime(DateTime.Today));

            Assert.Throws<InvalidOperationException>(() =>
                cycle.RegisterExpense(
                    10));
        }

        [Fact]
        public void AfterClosingDay_ExpenseGoesToNextOpenDay()
        {
            var source = FundingSource.Create("Vale Refeição");
            var cycle = BudgetCycle.Create(
                source,
                DateOnly.FromDateTime(DateTime.Today),
                3,
                300);

            cycle.CloseCurrentDay();

            cycle.RegisterExpense(
                50,
                "Almoço");

            var secondDay = cycle.Days.ElementAt(1);

            Assert.Equal(50, secondDay.TotalSpent.Amount);
        }

        [Fact]
        public void RegisterExpense_ShouldFail_WhenCycleIsFinished()
        {
            var source = FundingSource.Create("Cartão");
            var cycle = BudgetCycle.Create(
                source,
                DateOnly.FromDateTime(DateTime.Today),
                1,
                100);

            cycle.CloseCurrentDay();

            Assert.Throws<InvalidOperationException>(() =>
                cycle.RegisterExpense(
                    10));
        }

    }
}
