using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Infrastructure.Persistence;

namespace ShopManagementSystem.Infrastructure.Persistence.Seed;

public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Products.AnyAsync())
        {
            return;
        }

        var products = new List<Product>
        {
            new()
            {
                ProductId = "PRD-001",
                ProductName = "Wireless Mouse",
                CostPrice = 450.00m,
                Gst = 18.00m,
                Quantity = 50,
                PurchaseDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            },
            new()
            {
                ProductId = "PRD-002",
                ProductName = "Mechanical Keyboard",
                CostPrice = 2500.00m,
                Gst = 18.00m,
                Quantity = 30,
                PurchaseDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            },
            new()
            {
                ProductId = "PRD-003",
                ProductName = "USB-C Cable",
                CostPrice = 199.00m,
                Gst = 12.00m,
                Quantity = 100,
                PurchaseDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            }
        };

        var customers = new List<Customer>
        {
            new()
            {
                CustomerName = "Rahul Sharma",
                PhoneNumber = "9876543210",
                Address = "123 MG Road, Bangalore",
                CreatedDate = DateTime.UtcNow
            },
            new()
            {
                CustomerName = "Priya Patel",
                PhoneNumber = "9123456780",
                Address = "45 Park Street, Mumbai",
                CreatedDate = DateTime.UtcNow
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync();
    }
}
