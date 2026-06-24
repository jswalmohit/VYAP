namespace ShopManagementSystem.Application.DTOs.ProductTypes;

public class ProductTypeDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string HSN { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class CreateProductTypeDto
{
    public string Type { get; set; } = string.Empty;
    public string HSN { get; set; } = string.Empty;
}

public class UpdateProductTypeDto
{
    public string Type { get; set; } = string.Empty;
    public string HSN { get; set; } = string.Empty;
}
