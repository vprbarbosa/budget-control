using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Application.DTOs
{
    public sealed class AdjustBudgetCyclePeriodInput
    {
        public Guid CycleId { get; init; }
        public DateOnly EndDate { get; init; }
    }
}
