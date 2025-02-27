using Sales.Enums;

namespace Sales.Models;

public class Sale
{
    public int Id { get; set; }
    public ESaleStatus Status { get; set; }
    public DateTime Date { get; set; }
    public int OrderId { get; set; }

    public Seller Seller { get; set; }
    public List<Item> Items { get; set; }
}
