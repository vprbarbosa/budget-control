using BudgetControl.Api.DTOs;
using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Application.UseCases.CreateFundingSource
{
    public sealed class CreateFundingSourceUseCase
    {
        private readonly IFundingSourceRepository _repository;

        public CreateFundingSourceUseCase(IFundingSourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<FundingSourceDto> ExecuteAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("Funding source name is required.");

            var source = FundingSource.Create(name);

            await _repository.SaveAsync(source);

            return FundingSourceDto.From(source);
        }
    }

}
