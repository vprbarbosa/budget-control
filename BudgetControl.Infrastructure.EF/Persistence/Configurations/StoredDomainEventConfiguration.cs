using BudgetControl.Infrastructure.EF.Persistence.EventStore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetControl.Infrastructure.EF.Persistence.Configurations
{
    public sealed class StoredDomainEventConfiguration : IEntityTypeConfiguration<StoredDomainEvent>
    {
        public void Configure(EntityTypeBuilder<StoredDomainEvent> builder)
        {
            builder.ToTable("DomainEvents");

            builder.HasKey(e => e.EventId);

            builder.Property(e => e.AggregateType).IsRequired();
            builder.Property(e => e.EventType).IsRequired();
            builder.Property(e => e.PayloadJson).IsRequired();
        }
    }
}
