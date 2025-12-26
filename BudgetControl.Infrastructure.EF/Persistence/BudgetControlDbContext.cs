using BudgetControl.Domain.Aggregates;
using BudgetControl.Domain.Categories;
using BudgetControl.Domain.Entities;
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
        public DbSet<SpendingCategory> SpendingCategories => Set<SpendingCategory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetControlDbContext).Assembly);
        }
    }
}
