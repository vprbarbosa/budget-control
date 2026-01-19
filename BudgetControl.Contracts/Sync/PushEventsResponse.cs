using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Contracts.Sync
{
    public sealed class PushEventsResponse
    {
        public int Accepted { get; init; }

        public int Duplicates { get; init; }
    }
}
