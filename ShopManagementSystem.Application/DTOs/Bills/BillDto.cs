namespace ShopManagementSystem.Application.DTOs.Bills;

public class BillDto
{
    public int Id { get; set; }
    public string BillNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime BillDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal GstAmount { get; set; }
    public decimal GrandTotal { get; set; }
    public IList<BillItemDto> BillItems { get; set; } = new List<BillItemDto>();
}

public class BillItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Gst { get; set; }
    public decimal LineTotal { get; set; }
}

public class CreateBillItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CreateBillDto
{
    public int CustomerId { get; set; }
    public IList<CreateBillItemDto> Items { get; set; } = new List<CreateBillItemDto>();
}
