namespace ShopManagementSystem.Domain.Entities;

public class Customer
{
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? District { get; set; }
    public string? State { get; set; }
    public int? Pincode { get; set; }
    public DateTime CreatedDate { get; set; }
}
