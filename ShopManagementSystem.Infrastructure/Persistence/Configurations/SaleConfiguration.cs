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

        builder.Property(x => x.SellingPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.InvoiceNo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.SaleDate)
            .IsRequired();

        builder.Property(x => x.CreatedDate)
            .IsRequired();

        builder.HasIndex(x => x.InvoiceNo);
        builder.HasIndex(x => x.ProductId);
        builder.HasIndex(x => x.CustomerId);
        builder.HasIndex(x => x.SaleDate);
    }
}
