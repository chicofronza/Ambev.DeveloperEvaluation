using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    /// <summary>
    /// Entity Framework configuration for the SaleItem entity.
    /// </summary>
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        /// <summary>
        /// Configures the entity mapping.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItems");

            builder.HasKey(si => si.Id);
            builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(si => si.SaleId)
                .IsRequired();

            builder.Property(si => si.ProductId)
                .IsRequired();

            builder.Property(si => si.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(si => si.Quantity)
                .IsRequired();

            builder.Property(si => si.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(si => si.DiscountPercentage)
                .IsRequired()
                .HasColumnType("decimal(5,4)");

            builder.Property(si => si.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(si => si.IsCancelled)
                .IsRequired();

            builder.Property(si => si.CreatedAt)
                .IsRequired();

            builder.Property(si => si.UpdatedAt);
        }
    }
}