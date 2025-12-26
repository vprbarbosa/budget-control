using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Application.DTOs
{
    public sealed class DayExpenseDto
    {
        public Guid Id { get; init; }

        public decimal Amount { get; init; }

        public string Description { get; init; } = string.Empty;
    }

}
