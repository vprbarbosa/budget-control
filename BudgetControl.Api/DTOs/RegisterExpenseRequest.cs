using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Api.DTOs
{
    public sealed class RegisterExpenseRequest
    {
        public Guid BudgetCycleId { get; init; }
        public decimal Amount { get; init; }
        public string Description { get; init; } = string.Empty;
    }
}
