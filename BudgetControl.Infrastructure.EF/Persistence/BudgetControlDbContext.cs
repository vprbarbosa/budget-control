using BudgetControl.Domain.Aggregates;
using BudgetControl.Domain.Categories;
using BudgetControl.Domain.Entities;
using BudgetControl.Infrastructure.EF.Persistence.EventStore;
using Microsoft.EntityFrameworkCore;

namespace BudgetControl.Infrastructure.EF.Persistence
{
    public sealed class BudgetControlDbContext : DbContext
    {
        public BudgetControlDbContext(DbContextOptions<BudgetControlDbContext> options)
            : base(options)
        {
        }

        public DbSet<BudgetCycle> BudgetCycles => Set<BudgetCycle>();
        public DbSet<FundingSource> FundingSources => Set<FundingSource>();
        public DbSet<StoredDomainEvent> StoredDomainEvents => Set<StoredDomainEvent>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetControlDbContext).Assembly);
        }
    }
}
