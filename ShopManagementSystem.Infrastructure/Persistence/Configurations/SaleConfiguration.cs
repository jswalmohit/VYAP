namespace ShopManagementSystem.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagementSystem.Domain.Entities;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.USP)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.InvoiceNo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.UpdatedDate)
            .IsRequired();

        builder.Property(x => x.CreatedDate)
            .IsRequired();

        builder.Property(x => x.CGSTRate)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(x => x.SGSTRate)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(x => x.UpdatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.IPAddress)
            .HasMaxLength(50);

        builder.HasIndex(x => x.InvoiceNo);
        builder.HasIndex(x => x.ProductId);
        builder.HasIndex(x => x.CustomerId);
        builder.HasIndex(x => x.UpdatedDate);
    }
}
