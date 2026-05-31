using Microsoft.AspNetCore.Mvc;
using ShopManagementSystem.Application.Common;
using ShopManagementSystem.Application.DTOs.Bills;
using ShopManagementSystem.Application.Interfaces;

namespace ShopManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillsController : ControllerBase
{
    private readonly IBillService _billService;

    public BillsController(IBillService billService)
    {
        _billService = billService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BillDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<BillDto>>> Create([FromBody] CreateBillDto dto, CancellationToken cancellationToken)
    {
        var bill = await _billService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = bill.Id }, ApiResponse<BillDto>.Ok(bill, "Bill created successfully."));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BillDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BillDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var bills = await _billService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<BillDto>>.Ok(bills));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<BillDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BillDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        var bill = await _billService.GetByIdAsync(id, cancellationToken);
        return Ok(ApiResponse<BillDto>.Ok(bill));
    }

    [HttpGet("customer/{customerId:int}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BillDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BillDto>>>> GetByCustomerId(int customerId, CancellationToken cancellationToken)
    {
        var bills = await _billService.GetByCustomerIdAsync(customerId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<BillDto>>.Ok(bills));
    }
}
