using Microsoft.AspNetCore.Mvc;
using ShopManagementSystem.Application.Common;
using ShopManagementSystem.Application.DTOs.Invoices;
using ShopManagementSystem.Application.Interfaces;

namespace ShopManagementSystem.API.Controllers;

/// <summary>
/// Controller for handling invoice-related API endpoints
/// Follows REST conventions and returns standardized API responses
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
    }

    /// <summary>
    /// Fetches detailed invoice information including customer details and line items
    /// </summary>
    /// <param name="invoiceNo">The invoice number to fetch</param>
    /// <param name="cancellationToken">Cancellation token for async operations</param>
    /// <returns>Invoice details with customer and line item information</returns>
    /// <response code="200">Returns the invoice details successfully</response>
    /// <response code="400">If invoice number is invalid or empty</response>
    /// <response code="404">If invoice is not found</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet("{invoiceNo}")]
    [ProducesResponseType(typeof(ApiResponse<InvoiceDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<InvoiceDetailsDto>>> FetchInvoiceDetails(
        string invoiceNo,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(invoiceNo))
        {
            return BadRequest(ApiResponse<object>.Fail("Invoice number cannot be empty."));
        }

        var invoiceDetails = await _invoiceService.FetchInvoiceDetailsAsync(invoiceNo, cancellationToken);
        return Ok(ApiResponse<InvoiceDetailsDto>.Ok(invoiceDetails, "Invoice details fetched successfully."));
    }
}
