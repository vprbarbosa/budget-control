using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Application.DTOs
{
    public sealed class BudgetCycleListItemDto
    {
        public Guid Id { get; init; }

        // Identidade visual do ciclo (ex: "Vale alimentação (02/2026)")
        public string Title { get; init; } = string.Empty;

        // Continua existindo como dado contextual
        public string FundingSourceName { get; init; } = string.Empty;
    }

}
