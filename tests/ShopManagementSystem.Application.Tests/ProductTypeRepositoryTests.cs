using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Infrastructure.Persistence;
using ShopManagementSystem.Infrastructure.Repositories;
using Xunit;

namespace ShopManagementSystem.Application.Tests;

public class ProductTypeRepositoryTests
{
    private DbContextOptions<AppDbContext> CreateDbContextOptions()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        return new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
    }

    private async Task InitializeDatabaseAsync(DbContextOptions<AppDbContext> options)
    {
        using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();
    }

    [Fact]
    public async Task IsTypeUniqueAsync_ReturnsTrue_WhenTypeDoesNotExist()
    {
        var options = CreateDbContextOptions();
        await InitializeDatabaseAsync(options);

        using var context = new AppDbContext(options);
        var repository = new ProductTypeRepository(context);

        var result = await repository.IsTypeUniqueAsync("Electronics");

        Assert.True(result);
    }

    [Fact]
    public async Task IsTypeUniqueAsync_ReturnsFalse_WhenTypeExists()
    {
        var options = CreateDbContextOptions();
        await InitializeDatabaseAsync(options);

        using (var seedCtx = new AppDbContext(options))
        {
            seedCtx.ProductTypes.Add(new ProductType { Type = "Electronics", HSN = "85001000", CreatedDate = DateTime.UtcNow });
            await seedCtx.SaveChangesAsync();
        }

        using var context = new AppDbContext(options);
        var repository = new ProductTypeRepository(context);

        var result = await repository.IsTypeUniqueAsync("Electronics");

        Assert.False(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllProductTypes()
    {
        var options = CreateDbContextOptions();
        await InitializeDatabaseAsync(options);

        using (var seedCtx = new AppDbContext(options))
        {
            seedCtx.ProductTypes.AddRange(
                new ProductType { Type = "Electronics", HSN = "85001000", CreatedDate = DateTime.UtcNow },
                new ProductType { Type = "Clothing", HSN = "62001000", CreatedDate = DateTime.UtcNow }
            );
            await seedCtx.SaveChangesAsync();
        }

        using var context = new AppDbContext(options);
        var repository = new ProductTypeRepository(context);

        var result = await repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProductType_WhenExists()
    {
        var options = CreateDbContextOptions();
        await InitializeDatabaseAsync(options);

        int productTypeId;
        using (var seedCtx = new AppDbContext(options))
        {
            var productType = new ProductType { Type = "Electronics", HSN = "85001000", CreatedDate = DateTime.UtcNow };
            seedCtx.ProductTypes.Add(productType);
            await seedCtx.SaveChangesAsync();
            productTypeId = productType.Id;
        }

        using var context = new AppDbContext(options);
        var repository = new ProductTypeRepository(context);

        var result = await repository.GetByIdAsync(productTypeId);

        Assert.NotNull(result);
        Assert.Equal("Electronics", result.Type);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        var options = CreateDbContextOptions();
        await InitializeDatabaseAsync(options);

        using var context = new AppDbContext(options);
        var repository = new ProductTypeRepository(context);

        var result = await repository.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsProductType_Successfully()
    {
        var options = CreateDbContextOptions();
        await InitializeDatabaseAsync(options);

        using var context = new AppDbContext(options);
        var repository = new ProductTypeRepository(context);
        var productType = new ProductType { Type = "Electronics", HSN = "85001000", CreatedDate = DateTime.UtcNow };

        await repository.AddAsync(productType);
        await context.SaveChangesAsync();

        using var verifyCtx = new AppDbContext(options);
        var saved = await verifyCtx.ProductTypes.FirstOrDefaultAsync(pt => pt.Type == "Electronics");
        Assert.NotNull(saved);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesProductType_Successfully()
    {
        var options = CreateDbContextOptions();
        await InitializeDatabaseAsync(options);

        ProductType productType;
        using (var seedCtx = new AppDbContext(options))
        {
            productType = new ProductType { Type = "Electronics", HSN = "85001000", CreatedDate = DateTime.UtcNow };
            seedCtx.ProductTypes.Add(productType);
            await seedCtx.SaveChangesAsync();
        }

        using var context = new AppDbContext(options);
        var repository = new ProductTypeRepository(context);
        productType.Type = "Electronics Updated";

        await repository.UpdateAsync(productType);
        await context.SaveChangesAsync();

        using var verifyCtx = new AppDbContext(options);
        var updated = await verifyCtx.ProductTypes.FirstOrDefaultAsync(pt => pt.Id == productType.Id);
        Assert.NotNull(updated);
        Assert.Equal("Electronics Updated", updated.Type);
    }

    [Fact]
    public async Task DeleteAsync_DeletesProductType_Successfully()
    {
        var options = CreateDbContextOptions();
        await InitializeDatabaseAsync(options);

        ProductType productType;
        using (var seedCtx = new AppDbContext(options))
        {
            productType = new ProductType { Type = "Electronics", HSN = "85001000", CreatedDate = DateTime.UtcNow };
            seedCtx.ProductTypes.Add(productType);
            await seedCtx.SaveChangesAsync();
        }

        using var context = new AppDbContext(options);
        var repository = new ProductTypeRepository(context);

        await repository.DeleteAsync(productType);
        await context.SaveChangesAsync();

        using var verifyCtx = new AppDbContext(options);
        var deleted = await verifyCtx.ProductTypes.FirstOrDefaultAsync(pt => pt.Id == productType.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task FindAsync_ReturnsMatchingProductTypes()
    {
        var options = CreateDbContextOptions();
        await InitializeDatabaseAsync(options);

        using (var seedCtx = new AppDbContext(options))
        {
            seedCtx.ProductTypes.AddRange(
                new ProductType { Type = "Electronics", HSN = "85001000", CreatedDate = DateTime.UtcNow },
                new ProductType { Type = "Furniture", HSN = "94010000", CreatedDate = DateTime.UtcNow }
            );
            await seedCtx.SaveChangesAsync();
        }

        using var context = new AppDbContext(options);
        var repository = new ProductTypeRepository(context);

        var result = await repository.FindAsync(pt => pt.Type.Contains("Electronics"));

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsTrue_WhenPredicateMatches()
    {
        var options = CreateDbContextOptions();
        await InitializeDatabaseAsync(options);

        using (var seedCtx = new AppDbContext(options))
        {
            seedCtx.ProductTypes.Add(new ProductType { Type = "Electronics", HSN = "85001000", CreatedDate = DateTime.UtcNow });
            await seedCtx.SaveChangesAsync();
        }

        using var context = new AppDbContext(options);
        var repository = new ProductTypeRepository(context);

        var result = await repository.ExistsAsync(pt => pt.Type == "Electronics");

        Assert.True(result);
    }
}
