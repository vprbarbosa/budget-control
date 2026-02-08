using BudgetControl.Application.Abstractions.Clock;
using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Application.DTOs;
using System.Runtime.InteropServices;

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

            var referenceDate = input.Date ?? _clock.Today();

            cycle.RegisterExpense(
                amount: input.Amount,
                description: input.Description,
                targetDate: referenceDate
            );

            await _repository.SaveAsync(cycle);
        }
    }
}
