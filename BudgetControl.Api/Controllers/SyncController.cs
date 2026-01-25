using BudgetControl.Application.DTOs;
using BudgetControl.Application.UseCases.GetDayExpenses;
using BudgetControl.Contracts.Sync;
using BudgetControl.Infrastructure.EF.Persistence;
using BudgetControl.Infrastructure.EF.Persistence.EventStore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Api.Controllers
{
    [ApiController]
    [Route("sync")]
    public sealed class SyncController : ControllerBase
    {
        private readonly BudgetControlDbContext _db;

        public SyncController(BudgetControlDbContext db)
        {
            _db = db;
        }

        [HttpPost("push")]
        public async Task<ActionResult<PushEventsResponse>> Push(
            PushEventsRequest request,
            CancellationToken ct)
        {
            var accepted = 0;
            var duplicates = 0;

            foreach (var evt in request.Events)
            {
                var exists = await _db.StoredDomainEvents
                    .AnyAsync(e => e.EventId == evt.EventId, ct);

                if (exists)
                {
                    duplicates++;
                    continue;
                }

                _db.StoredDomainEvents.Add(new StoredDomainEvent
                {
                    EventId = evt.EventId,
                    ContextId = evt.ContextId,
                    AggregateId = evt.AggregateId,
                    AggregateType = evt.AggregateType,
                    EventType = evt.EventType,
                    OccurredAt = evt.OccurredAt,
                    DeviceId = evt.DeviceId,
                    PayloadJson = evt.PayloadJson,
                    Version = evt.Version,
                    ReceivedAt = DateTimeOffset.UtcNow
                });

                accepted++;
            }

            await _db.SaveChangesAsync(ct);

            return Ok(new PushEventsResponse
            {
                Accepted = accepted,
                Duplicates = duplicates
            });
        }
    }
}
