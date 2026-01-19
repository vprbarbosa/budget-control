using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Infrastructure.EF.Persistence.EventStore
{
    public sealed class StoredDomainEvent
    {
        [Key]
        public Guid EventId { get; set; }

        public Guid ContextId { get; set; }

        public Guid AggregateId { get; set; }

        [Required]
        public string AggregateType { get; set; } = default!;

        [Required]
        public string EventType { get; set; } = default!;

        public DateTimeOffset OccurredAt { get; set; }

        public Guid DeviceId { get; set; }

        [Required]
        public string PayloadJson { get; set; } = default!;

        public int Version { get; set; }

        public DateTimeOffset ReceivedAt { get; set; }
    }
}
