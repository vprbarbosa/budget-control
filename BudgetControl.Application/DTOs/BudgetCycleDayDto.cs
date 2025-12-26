using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Application.DTOs
{
    public sealed class BudgetCycleDayDto
    {
        public DateOnly Date { get; init; }
        public decimal TotalSpent { get; init; }
        public bool IsClosed { get; init; }
    }
}
