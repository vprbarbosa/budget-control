using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Api.DTOs
{
    public sealed class CreateBudgetCycleRequest
    {
        public Guid FundingSourceId { get; init; }
        public DateOnly StartDate { get; init; }
        public int EstimatedDurationInDays { get; init; }
        public decimal TotalCapacity { get; init; }
    }
}
