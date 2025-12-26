using BudgetControl.Api.DTOs;
using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Application.UseCases.CreateFundingSource;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Api.Controllers
{
    [ApiController]
    [Route("api/funding-sources")]
    public sealed class FundingSourcesController : ControllerBase
    {
        private readonly CreateFundingSourceUseCase _createUseCase;
        private readonly IFundingSourceRepository _repository;

        public FundingSourcesController(
            CreateFundingSourceUseCase createUseCase,
            IFundingSourceRepository repository)
        {
            _createUseCase = createUseCase;
            _repository = repository;
        }

        /// <summary>
        /// Cria uma nova fonte de recursos (cartão, vale, reserva, etc).
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(FundingSourceDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateFundingSourceInputDto input)
        {
            var result = await _createUseCase.ExecuteAsync(input.Name);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        /// <summary>
        /// Obtém uma fonte de recursos pelo ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(FundingSourceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var source = await _repository.GetByIdAsync(id);

            if (source is null)
                return NotFound();

            return Ok(FundingSourceDto.From(source));
        }
    }
}
