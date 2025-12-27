using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Web.Models
{
    public sealed class FundingSourceVm
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
