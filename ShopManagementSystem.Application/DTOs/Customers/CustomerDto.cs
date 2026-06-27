namespace ShopManagementSystem.Application.DTOs.Customers;

public class CustomerDto
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

public class CreateCustomerDto
{
    public string? CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? District { get; set; }
    public string? State { get; set; }
    public int? Pincode { get; set; }
}

public class UpdateCustomerDto
{
    public string CustomerName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? District { get; set; }
    public string? State { get; set; }
    public int? Pincode { get; set; }
}
