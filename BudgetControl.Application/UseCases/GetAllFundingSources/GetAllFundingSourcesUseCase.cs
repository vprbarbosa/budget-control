using BudgetControl.Api.DTOs;
using BudgetControl.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Application.UseCases.GetAllFundingSources
{
    public sealed class GetAllFundingSourcesUseCase
    {
        private readonly IFundingSourceRepository _repository;

        public GetAllFundingSourcesUseCase(
            IFundingSourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<FundingSourceDto>> ExecuteAsync()
        {
            var sources = await _repository.GetAllAsync();

            return sources
                .Select(s => new FundingSourceDto
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToList();
        }
    }
}
