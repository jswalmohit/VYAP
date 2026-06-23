namespace ShopManagementSystem.Application.DTOs.Sales;

public class SaleDto
{
    public int Id { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal USP { get; set; }
    public string InvoiceNo { get; set; } = string.Empty;
    public DateTime UpdatedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public decimal CGSTRate { get; set; }
    public decimal SGSTRate { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public string? IPAddress { get; set; }
}
