using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Application.DTOs
{
    public sealed class BudgetCycleListItemDto
    {
        public Guid Id { get; init; }
        public string FundingSourceName { get; init; } = string.Empty;
    }
}
