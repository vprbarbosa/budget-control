using BudgetControl.Application.UseCases.CloseDay;
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
        private readonly CloseDayUseCase _useCase;

        public DaysController(CloseDayUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost("{id:guid}/close-day")]
        public async Task<IActionResult> Close(Guid id)
        {
            try
            {
                await _useCase.ExecuteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

}
