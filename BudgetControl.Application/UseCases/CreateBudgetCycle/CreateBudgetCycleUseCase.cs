using BudgetControl.Application.DTOs;
using BudgetControl.Application.Interfaces;
using BudgetControl.Domain.Aggregates;
using BudgetControl.Domain.ValueObjects;

namespace BudgetControl.Application.UseCases.CreateBudgetCycle
{
    public sealed class CreateBudgetCycleUseCase
    {
        private readonly IBudgetCycleRepository _cycleRepository;
        private readonly IFundingSourceRepository _sourceRepository;

        public CreateBudgetCycleUseCase(
            IBudgetCycleRepository cycleRepository,
            IFundingSourceRepository sourceRepository)
        {
            _cycleRepository = cycleRepository;
            _sourceRepository = sourceRepository;
        }

        public async Task<BudgetCycleCreatedDto> ExecuteAsync(CreateBudgetCycleInput input)
        {
            var source = await _sourceRepository.GetByIdAsync(input.FundingSourceId)
                ?? throw new InvalidOperationException("Funding source not found.");

            var cycle = BudgetCycle.Create(
                source,
                input.StartDate,
                input.EstimatedDurationInDays,
                input.TotalCapacity);

            await _cycleRepository.AddAsync(cycle);
            await _cycleRepository.SaveChangesAsync();

            return BudgetCycleCreatedDto.From(cycle);
        }
    }
}
