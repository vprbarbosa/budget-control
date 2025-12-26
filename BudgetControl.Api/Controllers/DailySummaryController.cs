using BudgetControl.Application.UseCases.GetDailyBudgetSummary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Api.Controllers
{
    [ApiController]
    [Route("api/budget-cycles")]
    public sealed class DailySummaryController : ControllerBase
    {
        private readonly GetDailyBudgetSummaryUseCase _useCase;

        public DailySummaryController(GetDailyBudgetSummaryUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("{id:guid}/daily-summary")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var summary = await _useCase.ExecuteAsync(id);
                return Ok(summary);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }

}
