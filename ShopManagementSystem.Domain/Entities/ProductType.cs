namespace ShopManagementSystem.Domain.Entities;

public class ProductType
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string HSN { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
