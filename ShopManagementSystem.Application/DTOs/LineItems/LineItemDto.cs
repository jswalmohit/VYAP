namespace ShopManagementSystem.Application.DTOs.LineItems;

public class LineItemDto
{
    public Guid Id { get; set; }
    public int ProductId { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal Gst { get; set; }
    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? SellerGSTIN { get; set; }
    public string? SellerName { get; set; }
}

public class CreateLineItemDto
{
    public int ProductId { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal Gst { get; set; }
    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string? SellerGSTIN { get; set; }
    public string? SellerName { get; set; }
}

public class UpdateLineItemDto
{
    public int ProductId { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal Gst { get; set; }
    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string? SellerGSTIN { get; set; }
    public string? SellerName { get; set; }
}
