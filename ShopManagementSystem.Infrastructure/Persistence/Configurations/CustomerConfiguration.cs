using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagementSystem.Domain.Entities;

namespace ShopManagementSystem.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.CustomerId);

        builder.Property(c => c.CustomerId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.CustomerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(15);

        builder.HasIndex(c => c.PhoneNumber)
            .IsUnique();

        builder.Property(c => c.AddressLine1)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(c => c.AddressLine2)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(c => c.AddressLine3)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(c => c.District)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.Property(c => c.State)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.Property(c => c.Pincode)
            .IsRequired(false);

        builder.Property(c => c.CreatedDate)
            .IsRequired();
    }
}
