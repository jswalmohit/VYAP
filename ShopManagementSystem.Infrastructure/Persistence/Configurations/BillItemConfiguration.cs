using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagementSystem.Domain.Entities;

namespace ShopManagementSystem.Infrastructure.Persistence.Configurations;

public class BillItemConfiguration : IEntityTypeConfiguration<BillItem>
{
    public void Configure(EntityTypeBuilder<BillItem> builder)
    {
        builder.ToTable("BillItems");

        builder.HasKey(bi => bi.Id);

        builder.Property(bi => bi.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(bi => bi.Gst)
            .HasPrecision(5, 2);

        builder.Property(bi => bi.LineTotal)
            .HasPrecision(18, 2);

        builder.HasOne(bi => bi.Bill)
            .WithMany(b => b.BillItems)
            .HasForeignKey(bi => bi.BillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bi => bi.Product)
            .WithMany(p => p.BillItems)
            .HasForeignKey(bi => bi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
