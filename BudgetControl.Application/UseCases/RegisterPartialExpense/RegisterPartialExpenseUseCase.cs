using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Application.DTOs;

namespace BudgetControl.Application.UseCases.RegisterPartialExpense
{
    public sealed class RegisterPartialExpenseUseCase
    {
        private readonly IBudgetCycleRepository _cycleRepository;

        public RegisterPartialExpenseUseCase(
            IBudgetCycleRepository cycleRepository)
        {
            _cycleRepository = cycleRepository;
        }

        public async Task ExecuteAsync(RegisterPartialExpenseInput input)
        {
            var cycle = await _cycleRepository.GetByIdAsync(input.BudgetCycleId)
                ?? throw new InvalidOperationException("Cycle not found.");

            cycle.RegisterExpense(
                input.Amount,
                input.Description);

            await _cycleRepository.SaveAsync(cycle);
        }

    }
}
