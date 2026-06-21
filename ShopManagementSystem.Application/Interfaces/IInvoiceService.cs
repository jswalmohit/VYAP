using ShopManagementSystem.Application.DTOs.Invoices;

namespace ShopManagementSystem.Application.Interfaces;

/// <summary>
/// Service interface for invoice-related operations
/// </summary>
public interface IInvoiceService
{
    /// <summary>
    /// Fetches invoice details by invoice number
    /// </summary>
    /// <param name="invoiceNo">The invoice number to fetch details for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Invoice details including customer and line item information</returns>
    Task<InvoiceDetailsDto> FetchInvoiceDetailsAsync(string invoiceNo, CancellationToken cancellationToken = default);
}
