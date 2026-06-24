using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Application.DTOs.ProductTypes;
using ShopManagementSystem.Application.Exceptions;
using ShopManagementSystem.Application.Mappings;
using ShopManagementSystem.Application.Services;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Infrastructure.Persistence;
using ShopManagementSystem.Infrastructure.Repositories;
using Xunit;

namespace ShopManagementSystem.Application.Tests;

public class ProductTypeServiceTests
{
    private readonly IMapper _mapper;

    public ProductTypeServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

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
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoProductTypes()
    {
        var options = CreateDbContextOptions();
        await InitializeDatabaseAsync(options);

        using var context = new AppDbContext(options);
        using var unitOfWork = new UnitOfWork(context);
        var service = new ProductTypeService(unitOfWork, _mapper);

        var result = await service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
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
                new ProductType { Type = "Clothing", HSN = "62001000", CreatedDate = DateTime.UtcNow },
                new ProductType { Type = "Furniture", HSN = "94010000", CreatedDate = DateTime.UtcNow }
            );
            await seedCtx.SaveChangesAsync();
        }

        using var context = new AppDbContext(options);
        using var unitOfWork = new UnitOfWork(context);
        var service = new ProductTypeService(unitOfWork, _mapper);

        var result = await service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
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
        using var unitOfWork = new UnitOfWork(context);
        var service = new ProductTypeService(unitOfWork, _mapper);

        var result = await service.GetByIdAsync(productTypeId);

        Assert.NotNull(result);
        Assert.Equal("Electronics", result.Type);
        Assert.Equal("85001000", result.HSN);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFoundException_WhenNotExists()
    {
        var options = CreateDbContextOptions();
        await InitializeDatabaseAsync(options);

        using var context = new AppDbContext(options);
        using var unitOfWork = new UnitOfWork(context);
        var service = new ProductTypeService(unitOfWork, _mapper);

        async Task Act() => await service.GetByIdAsync(999);
        await Assert.ThrowsAsync<NotFoundException>(Act);
    }

    [Fact]
    public async Task CreateAsync_CreatesProductType_Successfully()
    {
        var options = CreateDbContextOptions();
        await InitializeDatabaseAsync(options);

        using var context = new AppDbContext(options);
        using var unitOfWork = new UnitOfWork(context);
        var service = new ProductTypeService(unitOfWork, _mapper);

        var createDto = new CreateProductTypeDto { Type = "Electronics", HSN = "85001000" };

        var result = await service.CreateAsync(createDto);

        Assert.NotNull(result);
        Assert.Equal("Electronics", result.Type);
        Assert.Equal("85001000", result.HSN);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task CreateAsync_ThrowsBusinessRuleException_WhenTypeAlreadyExists()
    {
        var options = CreateDbContextOptions();
        await InitializeDatabaseAsync(options);

        using (var seedCtx = new AppDbContext(options))
        {
            seedCtx.ProductTypes.Add(new ProductType { Type = "Electronics", HSN = "85001000", CreatedDate = DateTime.UtcNow });
            await seedCtx.SaveChangesAsync();
        }

        using var context = new AppDbContext(options);
        using var unitOfWork = new UnitOfWork(context);
        var service = new ProductTypeService(unitOfWork, _mapper);

        var createDto = new CreateProductTypeDto { Type = "Electronics", HSN = "85001001" };

        async Task Act() => await service.CreateAsync(createDto);
        await Assert.ThrowsAsync<BusinessRuleException>(Act);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesProductType_Successfully()
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
        using var unitOfWork = new UnitOfWork(context);
        var service = new ProductTypeService(unitOfWork, _mapper);

        var updateDto = new UpdateProductTypeDto { Type = "Electronics Updated", HSN = "85001001" };

        var result = await service.UpdateAsync(productTypeId, updateDto);

        Assert.NotNull(result);
        Assert.Equal("Electronics Updated", result.Type);
        Assert.NotNull(result.UpdatedDate);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsNotFoundException_WhenNotFound()
    {
        var options = CreateDbContextOptions();
        await InitializeDatabaseAsync(options);

        using var context = new AppDbContext(options);
        using var unitOfWork = new UnitOfWork(context);
        var service = new ProductTypeService(unitOfWork, _mapper);

        var updateDto = new UpdateProductTypeDto { Type = "Electronics", HSN = "85001000" };

        async Task Act() => await service.UpdateAsync(999, updateDto);
        await Assert.ThrowsAsync<NotFoundException>(Act);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsBusinessRuleException_WhenTypeAlreadyExists()
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
        using var unitOfWork = new UnitOfWork(context);
        var service = new ProductTypeService(unitOfWork, _mapper);

        var updateDto = new UpdateProductTypeDto { Type = "Clothing", HSN = "85001001" };

        var firstId = (await context.ProductTypes.FirstAsync()).Id;
        async Task Act() => await service.UpdateAsync(firstId, updateDto);
        await Assert.ThrowsAsync<BusinessRuleException>(Act);
    }
}
