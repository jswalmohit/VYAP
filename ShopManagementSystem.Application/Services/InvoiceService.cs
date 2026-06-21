using AutoMapper;
using ShopManagementSystem.Application.DTOs.Invoices;
using ShopManagementSystem.Application.Exceptions;
using ShopManagementSystem.Application.Interfaces;
using ShopManagementSystem.Domain.Interfaces;

namespace ShopManagementSystem.Application.Services;

/// <summary>
/// Service for handling invoice-related operations
/// Implements the Single Responsibility Principle by focusing solely on invoice functionality
/// </summary>
public class InvoiceService : IInvoiceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InvoiceService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Fetches complete invoice details including customer information and line items
    /// </summary>
    /// <param name="invoiceNo">The invoice number to fetch</param>
    /// <param name="cancellationToken">Cancellation token for async operations</param>
    /// <returns>Complete invoice details with customer and line item information</returns>
    /// <exception cref="NotFoundException">Thrown when invoice is not found</exception>
    public async Task<InvoiceDetailsDto> FetchInvoiceDetailsAsync(string invoiceNo, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(invoiceNo))
        {
            throw new ArgumentException("Invoice number cannot be empty.", nameof(invoiceNo));
        }

        // Fetch all sales for this invoice
        var sales = await _unitOfWork.Sales.GetByInvoiceNoAsync(invoiceNo, cancellationToken);
        
        if (!sales.Any())
        {
            throw new NotFoundException($"Invoice with number {invoiceNo} was not found.");
        }

        // Get the first sale to extract customer ID and date
        var firstSale = sales.First();

        // Fetch customer details
        var customer = await _unitOfWork.Customers.GetByPhoneNumberAsync(firstSale.CustomerId, cancellationToken);
        if (customer is null)
        {
            throw new NotFoundException($"Customer with ID {firstSale.CustomerId} was not found.");
        }

        // Prepare line items with product details
        var lineItems = await PrepareLineItemsAsync(sales, cancellationToken);

        // Calculate tax and total amounts
        var invoiceDetails = BuildInvoiceDetails(invoiceNo, firstSale, customer, lineItems);

        return invoiceDetails;
    }

    /// <summary>
    /// Prepares line items by fetching product details for each sale
    /// </summary>
    private async Task<IReadOnlyList<InvoiceLineItemDto>> PrepareLineItemsAsync(
        IReadOnlyList<Domain.Entities.Sale> sales,
        CancellationToken cancellationToken)
    {
        var lineItems = new List<InvoiceLineItemDto>();

        foreach (var sale in sales)
        {
            var product = await _unitOfWork.Products.GetByProductIdAsync(sale.ProductId, cancellationToken);
            
            if (product is null)
            {
                throw new NotFoundException($"Product with ID {sale.ProductId} was not found.");
            }

            var lineItem = new InvoiceLineItemDto
            {
                ProductId = sale.ProductId,
                ProductDescription = product.ProductName,
                HSN = product.HSN,
                Quantity = sale.Quantity,
                UnitPrice = product.SalePrice,
                CGST = product.Gst / 2, // Assuming CGST is half of total GST
                SGST = product.Gst / 2, // Assuming SGST is half of total GST
                Amount = sale.Quantity * sale.SellingPrice,
                Discount = 0 // Can be extended with discount logic
            };

            lineItems.Add(lineItem);
        }

        return lineItems;
    }

    /// <summary>
    /// Builds the complete invoice details with calculated totals and tax information
    /// </summary>
    private InvoiceDetailsDto BuildInvoiceDetails(
        string invoiceNo,
        Domain.Entities.Sale firstSale,
        Domain.Entities.Customer customer,
        IReadOnlyList<InvoiceLineItemDto> lineItems)
    {
        // Calculate amounts
        decimal amountBeforeTax = lineItems.Sum(li => li.Amount - (li.CGST + li.SGST));
        decimal cgstTotal = lineItems.Sum(li => li.CGST);
        decimal sgstTotal = lineItems.Sum(li => li.SGST);
        decimal gstTotal = cgstTotal + sgstTotal;
        decimal amountTotal = amountBeforeTax + gstTotal;

        // Calculate average tax rates
        decimal cgstRate = lineItems.Any() ? lineItems.Average(li => li.CGST) : 0;
        decimal sgstRate = lineItems.Any() ? lineItems.Average(li => li.SGST) : 0;

        var customerDetails = MapCustomerDetails(customer);

        var invoiceDetails = new InvoiceDetailsDto
        {
            InvoiceNumber = invoiceNo,
            Date = firstSale.SaleDate,
            AmountBeforeTax = amountBeforeTax,
            CGSTRate = cgstRate,
            SGSTRate = sgstRate,
            CGSTAmount = cgstTotal,
            SGSTAmount = sgstTotal,
            GSTTotal = gstTotal,
            AmountTotal = amountTotal,
            CustomerDetails = customerDetails,
            LineItems = lineItems
        };

        return invoiceDetails;
    }

    /// <summary>
    /// Maps customer entity to invoice customer details DTO
    /// Note: This assumes extended customer fields will be available in the future
    /// </summary>
    private InvoiceCustomerDetailsDto MapCustomerDetails(Domain.Entities.Customer customer)
    {
        return new InvoiceCustomerDetailsDto
        {
            Name = customer.CustomerName,
            Phone = customer.PhoneNumber,
            Email = string.Empty, // To be populated when Customer entity is extended
            GSTIN = string.Empty, // To be populated when Customer entity is extended
            Aadhar = string.Empty, // To be populated when Customer entity is extended
            AddressLine1 = customer.Address ?? string.Empty,
            AddressLine2 = string.Empty, // To be populated when Customer entity is extended
            AddressLine3 = string.Empty  // To be populated when Customer entity is extended
        };
    }
}
