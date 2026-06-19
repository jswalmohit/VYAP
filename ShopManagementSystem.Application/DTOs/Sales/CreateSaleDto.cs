using ShopManagementSystem.Application.DTOs.LineItems;

namespace ShopManagementSystem.Application.DTOs.Sales;

public class CreateSaleDto
{
    public required IEnumerable<SellProductDto> ProductList{ get; set; }
    public string CustomerId { get; set; } = string.Empty;
}
