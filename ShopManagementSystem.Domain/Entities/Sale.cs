namespace ShopManagementSystem.Domain.Entities;

public class Sale
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
