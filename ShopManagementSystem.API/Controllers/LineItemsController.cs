using Microsoft.AspNetCore.Mvc;
using ShopManagementSystem.Application.Common;
using ShopManagementSystem.Application.DTOs.LineItems;
using ShopManagementSystem.Application.Interfaces;

namespace ShopManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LineItemsController : ControllerBase
{
    private readonly ILineItemService _lineItemService;

    public LineItemsController(ILineItemService lineItemService)
    {
        _lineItemService = lineItemService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<LineItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<LineItemDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var lineItems = await _lineItemService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<LineItemDto>>.Ok(lineItems));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<LineItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<LineItemDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var lineItem = await _lineItemService.GetByIdAsync(id, cancellationToken);
        return Ok(ApiResponse<LineItemDto>.Ok(lineItem));
    }

    [HttpGet("product/{productId}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<LineItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<LineItemDto>>>> GetByProductId(string productId, CancellationToken cancellationToken)
    {
        var lineItems = await _lineItemService.GetByProductIdAsync(productId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<LineItemDto>>.Ok(lineItems));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LineItemDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<LineItemDto>>> Create([FromBody] CreateLineItemDto dto, CancellationToken cancellationToken)
    {
        var lineItem = await _lineItemService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = lineItem.Id }, ApiResponse<LineItemDto>.Ok(lineItem, "LineItem created successfully."));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<LineItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<LineItemDto>>> Update(Guid id, [FromBody] UpdateLineItemDto dto, CancellationToken cancellationToken)
    {
        var lineItem = await _lineItemService.UpdateAsync(id, dto, cancellationToken);
        return Ok(ApiResponse<LineItemDto>.Ok(lineItem, "LineItem updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _lineItemService.DeleteAsync(id, cancellationToken);
        return Ok(ApiResponse<object>.Ok(null!, "LineItem deleted successfully."));
    }
}
