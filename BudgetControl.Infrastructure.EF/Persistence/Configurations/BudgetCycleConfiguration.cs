using BudgetControl.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetControl.Infrastructure.EF.Persistence.Configurations
{
    internal sealed class BudgetCycleConfiguration
    : IEntityTypeConfiguration<BudgetCycle>
    {
        public void Configure(EntityTypeBuilder<BudgetCycle> builder)
        {
            builder.ToTable("budget_cycles");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.HasOne(x => x.Source)
                .WithMany()
                .HasForeignKey("FundingSourceId")
                .IsRequired();

            builder.Property(x => x.EndDate);

            builder.OwnsOne(x => x.Period, period =>
            {
                period.Property(p => p.StartDate)
                    .HasColumnName("start_date")
                    .IsRequired();

                period.Property(p => p.EstimatedDurationInDays)
                    .HasColumnName("estimated_duration_days")
                    .IsRequired();
            });

            builder.OwnsOne(x => x.TotalCapacity, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("total_capacity")
                    .HasPrecision(18, 2)
                    .IsRequired();
            });

            builder.Navigation(x => x.Days)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsMany(x => x.Days, dayBuilder =>
            {
                dayBuilder.ToTable("day_allocations");

                dayBuilder.WithOwner()
                    .HasForeignKey("BudgetCycleId");

                dayBuilder.HasKey("Id");

                dayBuilder.Property<Guid>("Id")
                    .ValueGeneratedNever();

                dayBuilder.Property(d => d.Date)
                    .HasColumnName("date")
                    .IsRequired();

                dayBuilder.Property(d => d.IsClosed)
                    .HasColumnName("is_closed")
                    .IsRequired();

                dayBuilder.OwnsMany(d => d.Expenses, exp =>
                {
                    exp.ToTable("partial_expenses");

                    exp.WithOwner()
                        .HasForeignKey("DayAllocationId");

                    exp.HasKey("Id");

                    exp.Property<Guid>("Id")
                        .ValueGeneratedNever();

                    exp.Property(e => e.Description)
                        .HasColumnName("description")
                        .HasMaxLength(600);

                    exp.OwnsOne(e => e.Amount, m =>
                    {
                        m.Property(x => x.Amount)
                            .HasColumnName("amount")
                            .HasPrecision(18, 2)
                            .IsRequired();
                    });
                });
            });

        }
    }
}