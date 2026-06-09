namespace ShopManagementSystem.Application.DTOs.LineItems;

public class LineItemDto
{
    public Guid Id { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public decimal PurchasePrice { get; set; }
    public decimal Gst { get; set; }
    public int Quantity { get; set; }
    public string SellerGSTIN { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? SellerInvoice { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class CreateLineItemDto
{
    public string ProductId { get; set; } = string.Empty;
    public decimal PurchasePrice { get; set; }
    public decimal Gst { get; set; }
    public int Quantity { get; set; }
    public string SellerGSTIN { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? SellerInvoice { get; set; }
    public DateTime PurchaseDate { get; set; }
}

public class UpdateLineItemDto
{
    public string ProductId { get; set; } = string.Empty;
    public decimal PurchasePrice { get; set; }
    public decimal Gst { get; set; }
    public int Quantity { get; set; }
    public string SellerGSTIN { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? SellerInvoice { get; set; }
    public DateTime PurchaseDate { get; set; }
}

public class UpdateLineItemBulkDto : UpdateLineItemDto
{
    public Guid Id { get; set; }
}

public class SellProductDto
{
    public string ProductId { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
