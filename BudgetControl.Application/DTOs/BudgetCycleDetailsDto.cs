using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Application.DTOs
{
    public sealed class BudgetCycleDetailsDto
    {
        public Guid Id { get; init; }
        public string FundingSourceName { get; init; } = string.Empty;

        public DateOnly StartDate { get; init; }
        public DateOnly EstimatedEndDate { get; init; }

        public decimal TotalCapacity { get; init; }
        public decimal TotalSpent { get; init; }
        public decimal RemainingCapacity { get; init; }

        public decimal DailyCapacity { get; init; }
        public int RemainingDays { get; init; }
    }

}
