using Microsoft.AspNetCore.Mvc;
using ShopManagementSystem.Application.Common;
using ShopManagementSystem.Application.DTOs.Sales;
using ShopManagementSystem.Application.Interfaces;

namespace ShopManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISalesService _salesService;

    public SalesController(ISalesService salesService)
    {
        _salesService = salesService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SaleDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<SaleDto>>> Create([FromBody] CreateSaleDto dto, CancellationToken cancellationToken)
    {
        var sale = await _salesService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = sale.Id }, ApiResponse<SaleDto>.Ok(sale, "Product sold successfully."));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SaleDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SaleDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var sales = await _salesService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<SaleDto>>.Ok(sales));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<SaleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SaleDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        var sale = await _salesService.GetByIdAsync(id, cancellationToken);
        return Ok(ApiResponse<SaleDto>.Ok(sale));
    }

    [HttpGet("product/{productId}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SaleDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SaleDto>>>> GetByProductId(string productId, CancellationToken cancellationToken)
    {
        var sales = await _salesService.GetByProductIdAsync(productId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<SaleDto>>.Ok(sales));
    }

    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SaleDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SaleDto>>>> GetByCustomerId(string customerId, CancellationToken cancellationToken)
    {
        var sales = await _salesService.GetByCustomerIdAsync(customerId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<SaleDto>>.Ok(sales));
    }

    [HttpGet("invoice/{invoiceNo}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SaleDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SaleDto>>>> GetByInvoiceNo(string invoiceNo, CancellationToken cancellationToken)
    {
        var sales = await _salesService.GetByInvoiceNoAsync(invoiceNo, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<SaleDto>>.Ok(sales));
    }

    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<SaleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<SaleDto>>> Update([FromBody] UpdateSaleDto dto, CancellationToken cancellationToken)
    {
        var sale = await _salesService.UpdateAsync(dto, cancellationToken);
        return Ok(ApiResponse<SaleDto>.Ok(sale, "Sale updated successfully."));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id, CancellationToken cancellationToken)
    {
        await _salesService.DeleteAsync(id, cancellationToken);
        return Ok(ApiResponse<object>.Ok(new { }, "Sale deleted successfully."));
    }
}
