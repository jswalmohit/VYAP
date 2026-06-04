using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagementSystem.Domain.Entities;

namespace ShopManagementSystem.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.ProductId)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.ProductId)
            .IsUnique();

        builder.Property(p => p.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.SellerGSTIN)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(p => p.SellerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.CostPrice)
            .HasPrecision(18, 2);

        builder.Property(p => p.Gst)
            .HasPrecision(5, 2);

        builder.Property(p => p.PurchaseDate)
            .IsRequired();

        builder.Property(p => p.CreatedDate)
            .IsRequired();
    }
}
