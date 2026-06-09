namespace ShopManagementSystem.Domain.Entities;

public class LineItem
{
    public Guid Id { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public int? BillId { get; set; }
    public string? Address { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal Gst { get; set; }
    public int Quantity { get; set; }
    public string SellerGSTIN { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public Product Product { get; set; } = null!;
    public Bill? Bill { get; set; }
}
