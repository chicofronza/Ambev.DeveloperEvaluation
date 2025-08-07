using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    /// <summary>
    /// Entity Framework configuration for the Sale entity.
    /// </summary>
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        /// <summary>
        /// Configures the entity mapping.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);
            builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(s => s.SaleNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.SaleDate)
                .IsRequired();

            builder.Property(s => s.CustomerId)
                .IsRequired();

            builder.Property(s => s.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.BranchId)
                .IsRequired();

            builder.Property(s => s.BranchName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.Status)
                .IsRequired();

            builder.Property(s => s.CreatedAt)
                .IsRequired();

            builder.Property(s => s.UpdatedAt);

            // Configure relationship with SaleItem
            builder.HasMany(s => s.Items)
                .WithOne()
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}