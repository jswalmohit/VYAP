namespace ShopManagementSystem.Application.DTOs.Sales;

public class CreateSaleDto
{
    public string ProductId { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal SellingPrice { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
}
