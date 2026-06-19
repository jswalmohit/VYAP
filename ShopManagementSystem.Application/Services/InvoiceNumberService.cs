using ShopManagementSystem.Application.Interfaces;

namespace ShopManagementSystem.Application.Services;

/// <summary>
/// Singleton service for managing and generating unique invoice numbers.
/// Thread-safe implementation ensures correct behavior in concurrent scenarios.
/// </summary>
public class InvoiceNumberService : IInvoiceNumberService
{
    private long _currentInvoiceNumber = 1020; // Initial invoice number
    private readonly object _lock = new();

    public string GetOrGenerateInvoiceNumber()
    {
        lock (_lock)
        {
            var invoiceNumber = $"INV-{_currentInvoiceNumber:D6}";
            _currentInvoiceNumber++;
            return invoiceNumber;
        }
    }
}
