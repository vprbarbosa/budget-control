using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Application.DTOs;

namespace BudgetControl.Application.UseCases.RegisterPartialExpense
{
    public sealed class RegisterPartialExpenseUseCase
    {
        private readonly IBudgetCycleRepository _cycleRepository;
        private readonly ISpendingCategoryRepository _categoryRepository;

        public RegisterPartialExpenseUseCase(
            IBudgetCycleRepository cycleRepository,
            ISpendingCategoryRepository categoryRepository)
        {
            _cycleRepository = cycleRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task ExecuteAsync(RegisterPartialExpenseInput input)
        {
            var cycle = await _cycleRepository.GetByIdAsync(input.BudgetCycleId)
                ?? throw new InvalidOperationException("Budget cycle not found.");

            var category = await _categoryRepository.GetByIdAsync(input.SpendingCategoryId)
                ?? throw new InvalidOperationException("Spending category not found.");

            cycle.RegisterExpense(
                input.Date,
                input.Amount,
                category,
                input.UserId,
                input.Description);

            await _cycleRepository.SaveAsync(cycle);
        }
    }
}
