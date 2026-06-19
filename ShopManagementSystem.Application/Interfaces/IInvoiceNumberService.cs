namespace ShopManagementSystem.Application.Interfaces;

public interface IInvoiceNumberService
{
    /// <summary>
    /// Gets or generates the next invoice number.
    /// If an invoice number hasn't been generated yet, it initializes with a default value.
    /// </summary>
    /// <returns>The next invoice number as a string.</returns>
    string GetOrGenerateInvoiceNumber();
}
