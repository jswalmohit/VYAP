namespace ShopManagementSystem.Domain.Entities;

public class BillItem
{
    public int Id { get; set; }
    public int BillId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Gst { get; set; }
    public decimal LineTotal { get; set; }

    public Bill Bill { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
