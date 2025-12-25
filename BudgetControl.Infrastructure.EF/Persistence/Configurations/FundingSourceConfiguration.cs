using BudgetControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetControl.Infrastructure.EF.Persistence.Configurations
{
    internal sealed class FundingSourceConfiguration : IEntityTypeConfiguration<FundingSource>
    {
        public void Configure(EntityTypeBuilder<FundingSource> builder)
        {
            builder.ToTable("funding_sources");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
