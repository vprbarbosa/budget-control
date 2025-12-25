using BudgetControl.Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetControl.Infrastructure.EF.Persistence.Configurations
{
    internal sealed class SpendingCategoryConfiguration : IEntityTypeConfiguration<SpendingCategory>
    {
        public void Configure(EntityTypeBuilder<SpendingCategory> builder)
        {
            builder.ToTable("spending_categories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
