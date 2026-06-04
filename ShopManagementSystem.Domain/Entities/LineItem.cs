namespace ShopManagementSystem.Domain.Entities;

public class LineItem
{
    public Guid Id { get; set; }
    public int ProductId { get; set; }
    public decimal PurchasePrice { get; set; }
    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? SellerGSTIN { get; set; }
    public string? SellerName { get; set; }

    public Product Product { get; set; } = null!;
}
