using BudgetControl.Application.Abstractions.Persistence;

namespace BudgetControl.Application.UseCases.CloseDay
{
    public sealed class CloseDayUseCase
    {
        private readonly IBudgetCycleRepository _cycleRepository;

        public CloseDayUseCase(IBudgetCycleRepository cycleRepository)
        {
            _cycleRepository = cycleRepository;
        }

        public async Task ExecuteAsync(Guid cycleId)
        {
            var cycle = await _cycleRepository.GetByIdAsync(cycleId)
                ?? throw new InvalidOperationException("Cycle not found.");

            cycle.CloseCurrentDay();

            await _cycleRepository.SaveAsync(cycle);
        }
    }

}
