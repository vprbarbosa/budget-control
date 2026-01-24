using BudgetControl.Application.DTOs;
using BudgetControl.Application.UseCases.GetDayExpenses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Api.Controllers
{
    [ApiController]
    [Route("api/budget-cycles")]
    public sealed class DaysController : ControllerBase
    {
        public DaysController()
        {
            
        }

        [HttpGet("{id}/days/{date}/expenses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyCollection<DayExpenseDto>>> GetDayExpenses(Guid id, DateOnly date, [FromServices] GetDayExpensesUseCase useCase)
        {
            try
            {
                var result = await useCase.ExecuteAsync(id, date);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }

}
