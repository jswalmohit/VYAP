namespace ShopManagementSystem.Application.DTOs.Sales;

public class SaleDto
{
    public int Id { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal SellingPrice { get; set; }
    public string InvoiceNo { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public DateTime CreatedDate { get; set; }
}
