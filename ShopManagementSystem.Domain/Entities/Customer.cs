namespace ShopManagementSystem.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public DateTime CreatedDate { get; set; }

    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
