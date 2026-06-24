using Microsoft.Extensions.DependencyInjection;
using ShopManagementSystem.Application.Interfaces;
using ShopManagementSystem.Domain.Interfaces;

namespace ShopManagementSystem.Application.Services;

/// <summary>
/// Singleton service for managing and generating unique invoice numbers.
/// Thread-safe implementation ensures correct behavior in concurrent scenarios.
/// Initializes from the highest invoice number in the Sales table.
/// </summary>
public class InvoiceNumberService : IInvoiceNumberService
{
    private long _currentInvoiceNumber;
    private readonly object _lock = new();

    public InvoiceNumberService(IServiceProvider serviceProvider)
    {
        _currentInvoiceNumber = InitializeFromDatabase(serviceProvider);
    }

    private long InitializeFromDatabase(IServiceProvider serviceProvider)
    {
        try
        {
            // Create a scope to access the scoped IUnitOfWork
            using (var scope = serviceProvider.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                
                // Get the highest invoice number from Sales table
                var sales = unitOfWork.Sales.GetAllAsync().Result;
                
                var maxInvoiceNo = sales
                    .Where(s => !string.IsNullOrEmpty(s.InvoiceNo))
                    .Select(s => ExtractInvoiceNumber(s.InvoiceNo))
                    .DefaultIfEmpty(0000) // Default to 0000 if no sales exist
                    .Max();

                return maxInvoiceNo + 1;
            }
        }
        catch
        {
            // If database access fails, fall back to default
            return 0001;
        }
    }

    private long ExtractInvoiceNumber(string invoiceNo)
    {
        // Extract numeric part from invoice number (e.g., "INV-001020" -> 1020)
        if (string.IsNullOrEmpty(invoiceNo))
            return 0;

        var numericPart = new string(invoiceNo.Where(char.IsDigit).ToArray());
        return long.TryParse(numericPart, out var number) ? number : 0;
    }

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
