using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Application.DTOs.LineItems;
using ShopManagementSystem.Application.Mappings;
using ShopManagementSystem.Application.Services;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Infrastructure.Persistence;
using ShopManagementSystem.Infrastructure.Repositories;
using Xunit;

namespace ShopManagementSystem.Application.Tests;

public class LineItemServiceConcurrencyTests
{
    [Fact]
    public async Task SellProductsAsync_ConcurrentSells_DoesNotOversell()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        try
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            await SeedDatabaseAsync(options);

            var mapper = CreateAutoMapper();
            var barrier = new System.Threading.Barrier(2);

            var task1 = Task.Run(async () => await ExecuteSellAsync(options, mapper, barrier, "P1", 7));
            var task2 = Task.Run(async () => await ExecuteSellAsync(options, mapper, barrier, "P1", 7));

            var results = await Task.WhenAll(task1, task2);

            var successCount = results.Count(r => r.succeeded);
            Assert.Equal(1, successCount);

            using var verifyContext = new AppDbContext(options);
            var finalProduct = await verifyContext.Products.FirstAsync(p => p.ProductId == "P1");
            var finalInventory = await verifyContext.LineItems.Where(li => li.ProductId == "P1").SumAsync(li => li.Quantity);

            Assert.Equal(3, finalProduct.Quantity);
            Assert.Equal(3, finalInventory);
        }
        finally
        {
            connection.Close();
        }
    }

    [Fact]
    public async Task CreateAsync_StoresSellerAndAddressFieldsInDatabase()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        try
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new AppDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();
                context.Products.Add(new Product
                {
                    ProductId = "P2",
                    ProductName = "Test Product 2",
                    Quantity = 0,
                    CostPrice = 10m,
                    SalePrice = 20m,
                    Gst = 18m,
                    CreatedDate = DateTime.UtcNow
                });
                await context.SaveChangesAsync();
            }

            var mapper = CreateAutoMapper();

            using (var context = new AppDbContext(options))
            using (var unitOfWork = new UnitOfWork(context))
            {
                var service = new LineItemService(unitOfWork, mapper);
                var createDto = new CreateLineItemDto
                {
                    ProductId = "P2",
                    PurchasePrice = 12.5m,
                    Gst = 18m,
                    Quantity = 1,
                    SellerGSTIN = "GST123",
                    SellerName = "Seller One",
                    Address = "123 Market Street",
                    SellerInvoice = "INV-1001",
                    PurchaseDate = DateTime.UtcNow
                };

                var created = await service.CreateAsync(createDto);

                Assert.Equal("Seller One", created.SellerName);
                Assert.Equal("123 Market Street", created.Address);
                Assert.Equal("INV-1001", created.SellerInvoice);
                Assert.Equal("GST123", created.SellerGSTIN);
            }

            using (var verifyContext = new AppDbContext(options))
            {
                var persisted = await verifyContext.LineItems.FirstAsync(li => li.ProductId == "P2");
                Assert.Equal("Seller One", persisted.SellerName);
                Assert.Equal("123 Market Street", persisted.Address);
                Assert.Equal("INV-1001", persisted.SellerInvoice);
                Assert.Equal("GST123", persisted.SellerGSTIN);
            }
        }
        finally
        {
            connection.Close();
        }
    }

    private static async Task SeedDatabaseAsync(DbContextOptions<AppDbContext> options)
    {
        using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var product = new Product
        {
            ProductId = "P1",
            ProductName = "Test Product",
            Quantity = 10,
            CostPrice = 1m,
            SalePrice = 2m,
            Gst = 0,
            CreatedDate = DateTime.UtcNow
        };

        context.Products.Add(product);
        context.LineItems.AddRange(
            new LineItem
            {
                Id = Guid.NewGuid(),
                ProductId = "P1",
                PurchasePrice = 1m,
                Gst = 0,
                Quantity = 5,
                SellerGSTIN = string.Empty,
                SellerName = string.Empty,
                PurchaseDate = DateTime.UtcNow.AddDays(-1),
                CreatedDate = DateTime.UtcNow.AddMinutes(-10)
            },
            new LineItem
            {
                Id = Guid.NewGuid(),
                ProductId = "P1",
                PurchasePrice = 1m,
                Gst = 0,
                Quantity = 5,
                SellerGSTIN = string.Empty,
                SellerName = string.Empty,
                PurchaseDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow.AddMinutes(-5)
            });

        await context.SaveChangesAsync();
    }

    private static IMapper CreateAutoMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        return config.CreateMapper();
    }

    private static async Task<(bool succeeded, Exception? exception)> ExecuteSellAsync(
        DbContextOptions<AppDbContext> options,
        IMapper mapper,
        System.Threading.Barrier barrier,
        string productId,
        int quantity)
    {
        using var context = new AppDbContext(options);
        using var unitOfWork = new UnitOfWork(context);
        var service = new LineItemService(unitOfWork, mapper);

        barrier.SignalAndWait();

        try
        {
            await service.SellProductsAsync(new[] { new SellProductDto { ProductId = productId, Quantity = quantity } });
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex);
        }
    }
}
