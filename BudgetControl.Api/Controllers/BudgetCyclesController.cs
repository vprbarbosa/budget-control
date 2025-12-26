using BudgetControl.Api.DTOs;
using BudgetControl.Application.DTOs;
using BudgetControl.Application.UseCases.CreateBudgetCycle;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Api.Controllers
{
    [ApiController]
    [Route("api/budget-cycles")]
    public sealed class BudgetCyclesController : ControllerBase
    {
        private readonly CreateBudgetCycleUseCase _useCase;

        public BudgetCyclesController(CreateBudgetCycleUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBudgetCycleRequest request)
        {
            try
            {
                var result = await _useCase.ExecuteAsync(
                    new CreateBudgetCycleInput
                    {
                        FundingSourceId = request.FundingSourceId,
                        StartDate = request.StartDate,
                        EstimatedDurationInDays = request.EstimatedDurationInDays,
                        TotalCapacity = request.TotalCapacity
                    });

                return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

}
