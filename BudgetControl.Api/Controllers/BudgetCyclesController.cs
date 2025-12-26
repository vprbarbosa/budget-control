using BudgetControl.Api.DTOs;
using BudgetControl.Application.DTOs;
using BudgetControl.Application.UseCases.CreateBudgetCycle;
using BudgetControl.Application.UseCases.GetBudgetCycleDays;
using BudgetControl.Application.UseCases.GetBudgetCycleDetails;
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BudgetCycleDetailsDto>> GetById(Guid id, [FromServices] GetBudgetCycleDetailsUseCase useCase)
        {
            try
            {
                var result = await useCase.ExecuteAsync(id);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/days")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyCollection<BudgetCycleDayDto>>> GetDays(Guid id, [FromServices] GetBudgetCycleDaysUseCase useCase)
        {
            try
            {
                var result = await useCase.ExecuteAsync(id);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

    }

}
