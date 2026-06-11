namespace ShopManagementSystem.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; } = 0m;
    public decimal Gst { get; set; }
    public int Quantity { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime CreatedDate { get; set; }

    public ICollection<LineItem> LineItems { get; set; } = new List<LineItem>();
}
