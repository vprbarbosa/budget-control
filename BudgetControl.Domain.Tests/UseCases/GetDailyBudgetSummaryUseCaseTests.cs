using BudgetControl.Application.Infrastructure.InMemory;
using BudgetControl.Application.UseCases.GetDailyBudgetSummary;
using BudgetControl.Domain.Aggregates;
using BudgetControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Domain.Tests.UseCases
{
    public sealed class GetDailyBudgetSummaryUseCaseTests
    {
        [Fact]
        public async Task Should_return_daily_summary_for_active_cycles()
        {
            // Arrange
            var sourceRepo = new InMemoryFundingSourceRepository();
            var cycleRepo = new InMemoryBudgetCycleRepository();
            var clock = new FakeClock(new DateOnly(2025, 1, 10));

            var source = FundingSource.Create("Refeição");
            sourceRepo.Add(source);

            var cycle = BudgetCycle.Create(source, new DateOnly(2025, 1, 1), 30, 3000m);
            await cycleRepo.SaveAsync(cycle);

            var useCase = new GetDailyBudgetSummaryUseCase(cycleRepo);

            // Act
            var summary = await useCase.ExecuteAsync(cycle.Id);

            // Assert
            Assert.Equal(100m, summary.DailyCapacity);
            Assert.Equal(3000m, summary.RemainingCapacity);
            Assert.Equal(30, summary.RemainingDays);
        }

    }
}
