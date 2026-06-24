using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopManagementSystem.API.Controllers;
using ShopManagementSystem.Application.Common;
using ShopManagementSystem.Application.DTOs.ProductTypes;
using ShopManagementSystem.Application.Exceptions;
using ShopManagementSystem.Application.Interfaces;
using Xunit;

namespace ShopManagementSystem.Application.Tests;

public class ProductTypesControllerTests
{
    private readonly Mock<IProductTypeService> _mockService;
    private readonly ProductTypesController _controller;

    public ProductTypesControllerTests()
    {
        _mockService = new Mock<IProductTypeService>();
        _controller = new ProductTypesController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithProductTypes()
    {
        var productTypes = new List<ProductTypeDto>
        {
            new() { Id = 1, Type = "Electronics", HSN = "85001000", CreatedDate = DateTime.UtcNow },
            new() { Id = 2, Type = "Clothing", HSN = "62001000", CreatedDate = DateTime.UtcNow }
        };

        _mockService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(productTypes);

        var result = await _controller.GetAll(CancellationToken.None);

        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
        var response = Assert.IsType<ApiResponse<IReadOnlyList<ProductTypeDto>>>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal(2, response.Data.Count);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithEmptyList()
    {
        var productTypes = new List<ProductTypeDto>();

        _mockService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(productTypes);

        var result = await _controller.GetAll(CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<IReadOnlyList<ProductTypeDto>>>(okResult.Value);
        Assert.NotNull(response.Data);
    }

    [Fact]
    public async Task GetById_ReturnsOkWithProductType()
    {
        var productType = new ProductTypeDto
        {
            Id = 1,
            Type = "Electronics",
            HSN = "85001000",
            CreatedDate = DateTime.UtcNow
        };

        _mockService.Setup(s => s.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(productType);

        var result = await _controller.GetById(1, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<ProductTypeDto>>(okResult.Value);
        Assert.Equal("Electronics", response.Data.Type);
    }

    [Fact]
    public async Task GetById_ThrowsNotFound()
    {
        _mockService.Setup(s => s.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException("Not found"));

        async Task Act() => await _controller.GetById(999, CancellationToken.None);
        await Assert.ThrowsAsync<NotFoundException>(Act);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        var createDto = new CreateProductTypeDto { Type = "Electronics", HSN = "85001000" };
        var createdDto = new ProductTypeDto
        {
            Id = 1,
            Type = "Electronics",
            HSN = "85001000",
            CreatedDate = DateTime.UtcNow
        };

        _mockService.Setup(s => s.CreateAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdDto);

        var result = await _controller.Create(createDto, CancellationToken.None);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(201, createdResult.StatusCode);
    }

    [Fact]
    public async Task Create_ThrowsBusinessRuleException()
    {
        var createDto = new CreateProductTypeDto { Type = "Electronics", HSN = "85001000" };

        _mockService.Setup(s => s.CreateAsync(createDto, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new BusinessRuleException("Duplicate"));

        async Task Act() => await _controller.Create(createDto, CancellationToken.None);
        await Assert.ThrowsAsync<BusinessRuleException>(Act);
    }

    [Fact]
    public async Task Update_ReturnsOk()
    {
        var updateDto = new UpdateProductTypeDto { Type = "Electronics V2", HSN = "85001001" };
        var updatedDto = new ProductTypeDto
        {
            Id = 1,
            Type = "Electronics V2",
            HSN = "85001001",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _mockService.Setup(s => s.UpdateAsync(1, updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedDto);

        var result = await _controller.Update(1, updateDto, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<ProductTypeDto>>(okResult.Value);
        Assert.Equal("Electronics V2", response.Data.Type);
    }

    [Fact]
    public async Task Update_ThrowsNotFound()
    {
        var updateDto = new UpdateProductTypeDto { Type = "Electronics", HSN = "85001000" };

        _mockService.Setup(s => s.UpdateAsync(999, updateDto, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException("Not found"));

        async Task Act() => await _controller.Update(999, updateDto, CancellationToken.None);
        await Assert.ThrowsAsync<NotFoundException>(Act);
    }

    [Fact]
    public async Task Update_ThrowsBusinessRuleException()
    {
        var updateDto = new UpdateProductTypeDto { Type = "Existing", HSN = "85001000" };

        _mockService.Setup(s => s.UpdateAsync(1, updateDto, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new BusinessRuleException("Duplicate"));

        async Task Act() => await _controller.Update(1, updateDto, CancellationToken.None);
        await Assert.ThrowsAsync<BusinessRuleException>(Act);
    }
}
