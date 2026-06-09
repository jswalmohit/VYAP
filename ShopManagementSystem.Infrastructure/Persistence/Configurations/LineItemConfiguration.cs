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
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(l => l.PurchasePrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(l => l.Gst)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(l => l.Quantity)
            .IsRequired();

        builder.Property(l => l.SellerGSTIN)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(l => l.SellerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.PurchaseDate)
            .IsRequired();

        builder.Property(l => l.CreatedDate)
            .IsRequired();

        builder.Property(l => l.BillId)
            .IsRequired(false);

        builder.Property(l => l.Address)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.HasOne(l => l.Product)
            .WithMany(p => p.LineItems)
            .HasForeignKey(l => l.ProductId)
            .HasPrincipalKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Bill)
            .WithMany()
            .HasForeignKey(l => l.BillId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
