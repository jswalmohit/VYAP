using Microsoft.AspNetCore.Mvc;
using ShopManagementSystem.Application.Common;
using ShopManagementSystem.Application.DTOs.ProductTypes;
using ShopManagementSystem.Application.Interfaces;

namespace ShopManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductTypesController : ControllerBase
{
    private readonly IProductTypeService _productTypeService;

    public ProductTypesController(IProductTypeService productTypeService)
    {
        _productTypeService = productTypeService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ProductTypeDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ProductTypeDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var productTypes = await _productTypeService.GetAllAsync(cancellationToken);
        var responseMsg = $"found {productTypes.Count} product types";
        return Ok(ApiResponse<IReadOnlyList<ProductTypeDto>>.Ok(productTypes, responseMsg));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<ProductTypeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ProductTypeDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        var productType = await _productTypeService.GetByIdAsync(id, cancellationToken);
        return Ok(ApiResponse<ProductTypeDto>.Ok(productType));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ProductTypeDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ProductTypeDto>>> Create([FromBody] CreateProductTypeDto dto, CancellationToken cancellationToken)
    {
        var productType = await _productTypeService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = productType.Id }, ApiResponse<ProductTypeDto>.Ok(productType, "ProductType created successfully"));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<ProductTypeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ProductTypeDto>>> Update(int id, [FromBody] UpdateProductTypeDto dto, CancellationToken cancellationToken)
    {
        var productType = await _productTypeService.UpdateAsync(id, dto, cancellationToken);
        return Ok(ApiResponse<ProductTypeDto>.Ok(productType, "ProductType updated successfully"));
    }
}
