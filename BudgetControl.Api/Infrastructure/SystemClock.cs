using BudgetControl.Application.Abstractions.Clock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Api.Infrastructure
{
    public sealed class SystemClock : IClock
    {
        public DateOnly Today()
            => DateOnly.FromDateTime(DateTime.Today);
    }
}
