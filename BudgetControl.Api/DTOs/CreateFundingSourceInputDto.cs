using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Api.DTOs
{
    public sealed class CreateFundingSourceInputDto
    {
        public string Name { get; init; } = string.Empty;
    }
}
