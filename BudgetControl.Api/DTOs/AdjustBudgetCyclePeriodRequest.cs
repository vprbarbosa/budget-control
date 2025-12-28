using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Api.DTOs
{
    public sealed class AdjustBudgetCyclePeriodRequest
    {
        public DateOnly EndDate { get; init; }
    }

}
