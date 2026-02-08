using BudgetControl.Api.DTOs;
using BudgetControl.Application.DTOs;
using BudgetControl.Application.UseCases.RegisterPartialExpense;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Api.Controllers
{
    [ApiController]
    [Route("api/expenses")]
    public sealed class ExpensesController : ControllerBase
    {
        private readonly RegisterPartialExpenseUseCase _useCase;

        public ExpensesController(RegisterPartialExpenseUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            [FromBody] RegisterExpenseRequest request)
        {
            try
            {
                await _useCase.ExecuteAsync(
                    new RegisterPartialExpenseInput
                    {
                        BudgetCycleId = request.BudgetCycleId,
                        Amount = request.Amount,
                        Description = request.Description,
                        Date = request.Date
                    });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

}
