using BudgetControl.Infrastructure.EF.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BudgetControl.Migrations.Persistence
{
    public sealed class BudgetControlDbContextFactory
    : IDesignTimeDbContextFactory<BudgetControlDbContext>
    {
        public BudgetControlDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BudgetControlDbContext>();

            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=budget_control;Username=budget;Password=budget;",
                b => b.MigrationsAssembly("BudgetControl.Migrations"));

            return new BudgetControlDbContext(optionsBuilder.Options);
        }
    }
}
