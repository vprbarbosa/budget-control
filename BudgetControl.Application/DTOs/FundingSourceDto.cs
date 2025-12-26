using BudgetControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Api.DTOs
{
    public sealed class FundingSourceDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;

        public static FundingSourceDto From(FundingSource source)
            => new()
            {
                Id = source.Id,
                Name = source.Name
            };
    }

}
