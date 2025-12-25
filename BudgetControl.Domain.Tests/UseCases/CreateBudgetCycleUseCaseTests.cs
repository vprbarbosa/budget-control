using BudgetControl.Application.DTOs;
using BudgetControl.Application.Infrastructure.InMemory;
using BudgetControl.Application.UseCases.CreateBudgetCycle;
using BudgetControl.Domain.Entities;

namespace BudgetControl.Domain.Tests.UseCases
{
    public sealed class CreateBudgetCycleUseCaseTests
    {
        [Fact]
        public async Task Should_create_budget_cycle_with_correct_daily_capacity()
        {
            // Arrange
            var sourceRepo = new InMemoryFundingSourceRepository();
            var cycleRepo = new InMemoryBudgetCycleRepository();

            var source = FundingSource.Create("Cartão de crédito");
            sourceRepo.Add(source);

            var useCase = new CreateBudgetCycleUseCase(cycleRepo, sourceRepo);

            var input = new CreateBudgetCycleInput
            {
                FundingSourceId = source.Id,
                StartDate = new DateOnly(2025, 1, 1),
                EstimatedDurationInDays = 30,
                TotalCapacity = 3000m
            };

            // Act
            var result = await useCase.ExecuteAsync(input);

            // Assert
            var cycle = cycleRepo.All().Single();

            Assert.Equal(source.Id, cycle.Source.Id);
            Assert.Equal(30, cycle.Days.Count);
            Assert.Equal(100m, cycle.DailyCapacity.Amount);
            Assert.Equal(3000m, cycle.TotalCapacity.Amount);
        }
    }
}
