namespace ShopManagementSystem.Application.DTOs.Invoices;

/// <summary>
/// Represents the customer details within an invoice
/// </summary>
public class InvoiceCustomerDetailsDto
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string GSTIN { get; set; } = string.Empty;
    public string Aadhar { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string AddressLine3 { get; set; } = string.Empty;
}

/// <summary>
/// Represents a line item in an invoice
/// </summary>
public class InvoiceLineItemDto
{
    public string ProductId { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
    public string HSN { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal CGSTRate { get; set; }
    public decimal SGSTRate { get; set; }
    public decimal CGSTAmt => (CGSTRate/100) * UnitPrice;
    public decimal SGSTAmt => (SGSTRate/100) * UnitPrice;
    public decimal GSTTotal => CGSTAmt + SGSTAmt;
    public decimal AmountBeforeTax => Quantity * UnitPrice;
    public decimal AmountWithGST => AmountBeforeTax + GSTTotal;
    public decimal Discount { get; set; }
}

/// <summary>
/// Represents complete invoice details with customer and line item information
/// </summary>
public class InvoiceDetailsDto
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal AmountBeforeTax { get; set; }
    public decimal? CGSTRate { get; set; }
    public decimal? SGSTRate { get; set; }
    public decimal CGSTAmount { get; set; }
    public decimal SGSTAmount { get; set; }
    public decimal GSTTotal { get; set; }
    public decimal AmountTotal { get; set; }
    public InvoiceCustomerDetailsDto CustomerDetails { get; set; } = new();
    public IReadOnlyList<InvoiceLineItemDto> LineItems { get; set; } = new List<InvoiceLineItemDto>();
}
