using BudgetControl.Api.DTOs;
using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Application.UseCases.CreateFundingSource;
using BudgetControl.Application.UseCases.GetAllFundingSources;
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
        private readonly ILogger<FundingSourcesController> _logger;

        public FundingSourcesController(
            CreateFundingSourceUseCase createUseCase,
            IFundingSourceRepository repository,
            ILogger<FundingSourcesController> logger)
        {
            _createUseCase = createUseCase;
            _repository = repository;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(FundingSourceDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(
            [FromBody] CreateFundingSourceInputDto input)
        {
            try
            {
                var result = await _createUseCase.ExecuteAsync(input.Name);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.Id },
                    result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao criar FundingSource");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar FundingSource");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(FundingSourceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var source = await _repository.GetByIdAsync(id);

                if (source is null)
                    return NotFound();

                return Ok(FundingSourceDto.From(source));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar FundingSource {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FundingSourceDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(
            [FromServices] GetAllFundingSourcesUseCase useCase)
        {
            try
            {
                var result = await useCase.ExecuteAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar FundingSources");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
