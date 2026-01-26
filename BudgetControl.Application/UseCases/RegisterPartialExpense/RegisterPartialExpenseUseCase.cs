using BudgetControl.Application.Abstractions.Clock;
using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Application.DTOs;

namespace BudgetControl.Application.UseCases.RegisterPartialExpense
{
    public sealed class RegisterPartialExpenseUseCase
    {
        private readonly IBudgetCycleRepository _repository;
        private readonly IClock _clock;

        public RegisterPartialExpenseUseCase(
            IBudgetCycleRepository repository,
            IClock clock)
        {
            _repository = repository;
            _clock = clock;
        }

        public async Task ExecuteAsync(RegisterPartialExpenseInput input)
        {
            var cycle = await _repository.GetByIdAsync(input.BudgetCycleId)
                ?? throw new InvalidOperationException("Budget cycle not found.");

            var today = _clock.Today();

            cycle.RegisterExpense(
                amount: input.Amount,
                description: input.Description,
                today: today
            );

            await _repository.SaveAsync(cycle);
        }
    }
}
