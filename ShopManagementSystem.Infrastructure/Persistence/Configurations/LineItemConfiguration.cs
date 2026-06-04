using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagementSystem.Domain.Entities;

namespace ShopManagementSystem.Infrastructure.Persistence.Configurations;

public class LineItemConfiguration : IEntityTypeConfiguration<LineItem>
{
    public void Configure(EntityTypeBuilder<LineItem> builder)
    {
        builder.ToTable("LineItems");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.ProductId)
            .IsRequired();

        builder.Property(l => l.PurchasePrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(l => l.Quantity)
            .IsRequired();

        builder.Property(l => l.PurchaseDate)
            .IsRequired();

        builder.Property(l => l.CreatedDate)
            .IsRequired();

        builder.Property(l => l.SellerGSTIN)
            .HasMaxLength(50);

        builder.Property(l => l.SellerName)
            .HasMaxLength(200);

        builder.HasOne(l => l.Product)
            .WithMany(p => p.LineItems)
            .HasForeignKey(l => l.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
