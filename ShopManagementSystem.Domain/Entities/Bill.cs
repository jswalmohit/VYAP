namespace ShopManagementSystem.Domain.Entities;

public class Bill
{
    public int Id { get; set; }
    public string BillNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public DateTime BillDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal GstAmount { get; set; }
    public decimal GrandTotal { get; set; }

    public Customer Customer { get; set; } = null!;
    public ICollection<BillItem> BillItems { get; set; } = new List<BillItem>();
}
