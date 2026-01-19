using BudgetControl.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Contracts.Sync
{
    public sealed class PushEventsRequest
    {
        public required IReadOnlyCollection<DomainEventEnvelope> Events { get; init; }
    }
}
